let connection = null;
let currentPage = 1;
let isLoading = false;
let hasMore = true;
let totalPages = 1;

function initDiscussions() {
    const messagesContainer = document.getElementById('messagesList');
    if (!messagesContainer) return;

    loadLastPage();
    startConnection();

    const textarea = document.getElementById('messageContent');
    if (textarea) {
        textarea.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                sendMessage();
            }
        });
    }

    messagesContainer.addEventListener('scroll', function () {
        if (this.scrollTop === 0 && !isLoading && hasMore) {
            loadPreviousPage();
        }
    });
}

async function loadLastPage() {
    const container = document.getElementById('messagesList');
    container.innerHTML = '<div class="text-center text-muted py-5">Loading...</div>';

    try {
        const response = await fetch(`/Discussions/GetDiscussions?inventoryId=${window.inventoryId}&page=1&pageSize=20`);
        const data = await response.json();

        totalPages = data.totalPages || 1;

        if (totalPages === 0 || data.totalCount === 0) {
            container.innerHTML = `
                <p>No messages yet</p>
            `;
            return;
        }

        const messagesToShow = [];
        let remainingToLoad = 20;
        let page = totalPages;

        while (remainingToLoad > 0 && page >= 1) {
            const pageResponse = await fetch(`/Discussions/GetDiscussions?inventoryId=${window.inventoryId}&page=${page}&pageSize=20`);
            const pageData = await pageResponse.json();

            if (pageData.items && pageData.items.length > 0) {
                messagesToShow.unshift(...pageData.items);
                remainingToLoad -= pageData.items.length;
            }
            page--;
        }

        if (messagesToShow.length > 0) {
            container.innerHTML = '';
            messagesToShow.forEach(msg => addMessageToUI(msg));

            setTimeout(() => {
                container.scrollTop = container.scrollHeight;
            }, 100);

            hasMore = page >= 1;
            currentPage = page + 1;
        }
    } catch (error) {
        console.error('Error loading messages:', error);
        container.innerHTML = '<div class="text-center text-danger py-5">Error loading messages</div>';
    }
}

async function loadPreviousPage() {
    if (isLoading || !hasMore) return;

    isLoading = true;
    const prevPage = currentPage - 1;

    try {
        const response = await fetch(`/Discussions/GetDiscussions?inventoryId=${window.inventoryId}&page=${prevPage}&pageSize=20`);
        const data = await response.json();

        if (data.items && data.items.length > 0) {
            const reversedItems = [...data.items].reverse();
            addMessagesToBeginning(reversedItems);
            hasMore = prevPage > 1;
            currentPage = prevPage;
        } else {
            hasMore = false;
        }
    } catch (error) {
        console.error('Error loading previous page:', error);
    } finally {
        isLoading = false;
    }
}

function addMessagesToBeginning(messages) {
    const container = document.getElementById('messagesList');
    const scrollHeight = container.scrollHeight;
    const scrollTop = container.scrollTop;

    messages.forEach(msg => {
        const html = createMessageHtml(msg);
        container.insertAdjacentHTML('afterbegin', html);
    });

    const newScrollHeight = container.scrollHeight;
    container.scrollTop = scrollTop + (newScrollHeight - scrollHeight);
}

function createMessageHtml(message) {
    const date = new Date(message.audit.createdAt);
    const formattedDate = !isNaN(date.getTime()) ? date.toLocaleString() : message.audit.createdAt;

    return `
        <div class="mb-3 pb-2 border-bottom" data-message-id="${message.basic.id}">
            <div class="d-flex justify-content-between align-items-center">
                <strong class="text-primary">${escapeHtml(message.author.authorName)}</strong>
                <small class="text-muted">${escapeHtml(formattedDate)}</small>
            </div>
            <p class="mb-1 mt-1 text-dark">${escapeHtml(message.basic.content)}</p>
        </div>
    `;
}

function startConnection() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/discussionHub")
        .build();

    connection.start()
        .then(() => {
            connection.invoke("JoinInventoryGroup", window.inventoryId);
        })
        .catch(err => console.error("SignalR error:", err));

    connection.on("ReceiveMessage", (message) => {
        addMessageToUI(message);
        const container = document.getElementById('messagesList');
        container.scrollTop = container.scrollHeight;
    });
}

function sendMessage() {
    const textarea = document.getElementById('messageContent');
    const content = textarea?.value.trim();

    if (!content) return;

    fetch('/Discussions/Create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
        },
        body: JSON.stringify({
            inventoryId: window.inventoryId,
            content: content
        })
    }).then(response => {
        if (response.ok) {
            textarea.value = '';
        }
    }).catch(err => console.error("Error sending message:", err));
}

function addMessageToUI(message) {
    const container = document.getElementById('messagesList');
    if (!container) return;

    if (container.innerHTML.includes('No messages yet') || container.innerHTML.includes('Loading')) {
        container.innerHTML = '';
    }

    const html = createMessageHtml(message);
    container.insertAdjacentHTML('beforeend', html);
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

document.addEventListener('DOMContentLoaded', function () {
    if (window.inventoryId) {
        initDiscussions();
    }
});