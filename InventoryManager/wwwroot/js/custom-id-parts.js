let partSortable;
let autoSaveTimeout;
let previewTimeout;

document.addEventListener('DOMContentLoaded', () => {
    initPartSortable();
    updatePreview();
});

function scheduleAutoSave() {
    clearTimeout(autoSaveTimeout);
    autoSaveTimeout = setTimeout(saveParts, 7000);
}

function schedulePreview() {
    clearTimeout(previewTimeout);
    previewTimeout = setTimeout(updatePreview, 500);
}

async function saveParts() {
    const form = document.getElementById('customIdForm');
    if (!form) return;

    const formData = new FormData(form);
    const indicator = document.getElementById('partsSaveIndicator');
    if (indicator) {
        indicator.innerHTML = 'Saving...';
        indicator.className = 'badge bg-warning text-dark';
    }

    try {
        const response = await fetch('/Inventories/UpdateCustomIdParts', {
            method: 'POST',
            body: formData
        });

        if (response.ok && indicator) {
            indicator.innerHTML = 'Saved';
            indicator.className = 'badge bg-success';
            setTimeout(() => {
                indicator.innerHTML = 'Autosave enabled';
                indicator.className = 'badge bg-secondary';
            }, 2000);
        } else {
            const html = await response.text();
            updateForm(html);
            if (indicator) {
                indicator.innerHTML = 'Please fix errors';
                indicator.className = 'badge bg-danger';
                setTimeout(() => {
                    indicator.innerHTML = 'Autosave enabled';
                    indicator.className = 'badge bg-secondary';
                }, 3000);
            }
        }
    } catch (error) {
        console.error('Error:', error);
        spinner.classList.add('d-none');
    }
}

function updateForm(html) {
    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = html;
    const newForm = tempDiv.querySelector('#customIdForm');
    if (newForm) {
        const currentForm = document.getElementById('customIdForm');
        currentForm.innerHTML = newForm.innerHTML;
        initPartSortable();
    }
}

function initPartSortable() {
    const container = document.getElementById('partsContainer');
    if (container) {
        if (partSortable) partSortable.destroy();
        partSortable = new Sortable(container, {
            animation: 150,
            handle: '.drag-handle',
            onEnd: () => {
                reindexParts();
                updatePreview();
                scheduleAutoSave();
            }
        });
    }
}

function reindexParts() {
    document.querySelectorAll('.part-item').forEach((el, newOrder) => {
        el.setAttribute('data-part-order', newOrder);
        el.querySelectorAll('input, select, textarea').forEach(input => {
            if (input.name) {
                input.name = input.name.replace(/CustomIdParts\[\d+\]/, `CustomIdParts[${newOrder}]`);
            }
        });
        const orderInput = el.querySelector('input[name$=".Order"]');
        if (orderInput) {
            orderInput.value = newOrder;
        }
    });
}

function addPart(type) {
    const container = document.getElementById('partsContainer');
    const formData = new FormData();
    formData.append('type', type);
    formData.append('inventoryId', window.inventoryId);

    fetch('/Inventories/GetCustomIdPartHtml', {
        method: 'POST',
        body: formData
    })
    .then(res => res.text())
    .then(html => {
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = html;
        const newPart = tempDiv.querySelector('.part-item');
        if (newPart) {
            container.appendChild(newPart);
            reindexParts();
            updatePreview();
            initPartSortable();
            scheduleAutoSave();
        }
    })
    .catch(error => console.error('Error:', error));
}

let previewTimer;
function updatePreview() {
    clearTimeout(previewTimer);
    const form = document.getElementById('customIdForm');
    const hasParts = document.querySelectorAll('.part-item').length > 0;
    const preview = document.getElementById('customIdPreview');
    if (!hasParts) {
        if (preview) preview.innerHTML = 'Not generated yet';
        return;
    }
    previewTimer = setTimeout(async () => {
        try {
            const res = await fetch('/Inventories/PreviewCustomId', {
                method: 'POST',
                body: new FormData(form)
            });
            if (preview) preview.innerHTML = `<code>${await res.text()}</code>`;
        } catch {
            if (preview) preview.innerHTML = '<span class="text-danger">Error</span>';
        }
    }, 300);
}

function removePart(btn) {
    btn.closest('.part-item')?.remove();
    reindexParts();
    updatePreview();
    scheduleAutoSave();
}

window.scheduleAutoSave = scheduleAutoSave;