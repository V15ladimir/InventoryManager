let fieldSortable;
let fieldAutoSaveTimeout;

document.addEventListener('DOMContentLoaded', () => {
    initFieldSortable();
    reindexFields();
});

function scheduleFieldAutoSave() {
    clearTimeout(fieldAutoSaveTimeout);
    fieldAutoSaveTimeout = setTimeout(saveFields, 7000);
}

async function saveFields() {
    const form = document.getElementById('customFieldsForm');
    if (!form) return;

    const formData = new FormData(form);
    const indicator = document.getElementById('fieldsSaveIndicator');
    if (indicator) {
        indicator.innerHTML = 'Saving...';
        indicator.className = 'badge bg-warning mb-3';
    }

    try {
        const response = await fetch('/Inventories/UpdateCustomFields', {
            method: 'POST',
            body: formData
        });
        if (response.ok && indicator) {
            const html = await response.text();
            updateFieldsForm(html); 
            const newIndicator = document.getElementById('fieldsSaveIndicator');
            newIndicator.innerHTML = 'Saved';
            newIndicator.className = 'badge bg-success mb-3';
            setTimeout(() => {
                newIndicator.innerHTML = 'Autosave enabled';
                newIndicator.className = 'badge bg-secondary mb-3';
            }, 7000);
        } else {
            const html = await response.text();
            updateFieldsForm(html);
            const newIndicator = document.getElementById('fieldsSaveIndicator');
            newIndicator.innerHTML = 'Error';
            newIndicator.className = 'badge bg-danger mb-3';
            setTimeout(() => {
                newIndicator.innerHTML = 'Autosave enabled';
                newIndicator.className = 'badge bg-secondary mb-3';
            }, 7000);
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

function updateFieldsForm(html) {
    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = html;
    const newForm = tempDiv.querySelector('#customFieldsForm');

    if (newForm) {
        const currentForm = document.getElementById('customFieldsForm');
        currentForm.innerHTML = newForm.innerHTML;
        reindexFields();
        initFieldSortable();
    }
}

function reindexFields() {
    document.querySelectorAll('.field-item').forEach((el, newOrder) => {
        el.setAttribute('data-field-order', newOrder);
        el.querySelectorAll('input, select, textarea').forEach(input => {
            if (input.name) {
                input.name = input.name.replace(/CustomFields\[\d+\]/, `CustomFields[${newOrder}]`);
            }
        });

        const orderInput = el.querySelector('input[name$=".Order"]');
        if (orderInput) {
            orderInput.value = newOrder;
            orderInput.name = `CustomFields[${newOrder}].Order`;
        }

        //const typeInput = el.querySelector('input[name$=".Type"]');
        //if (typeInput) {
        //    typeInput.name = `CustomFields[${newOrder}].Type`;
        //}
    });
}

function initFieldSortable() {
    const container = document.getElementById('fieldsContainer');
    if (container) {
        if (fieldSortable) fieldSortable.destroy();
        fieldSortable = new Sortable(container, {
            animation: 150,
            handle: '.drag-handle',
            onEnd: () => {
                reindexFields();
                scheduleFieldAutoSave();
            }
        });
    }
}

function addField(type) {
    const container = document.getElementById('fieldsContainer');
    const formData = new FormData();
    formData.append('type', type);
    formData.append('inventoryId', window.inventoryId);

    fetch('/Inventories/GetCustomFieldHtml', {
        method: 'POST',
        body: formData
    })
    .then(res => res.text())
    .then(html => {
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = html;
        const newField = tempDiv.querySelector('.field-item');

        if (newField) {
            container.appendChild(newField);
            reindexFields();
            initFieldSortable();
            scheduleFieldAutoSave();
        }
    })
    .catch(error => console.error('Error:', error));
}

function removeField(btn) {
    btn.closest('.field-item')?.remove();
    reindexFields();
    scheduleFieldAutoSave();
}

window.scheduleFieldAutoSave = scheduleFieldAutoSave;