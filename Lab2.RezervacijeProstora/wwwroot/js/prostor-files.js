(function () {
    function getAntiForgeryToken(container) {
        var tokenInput = container.querySelector('input[name="__RequestVerificationToken"]');
        return tokenInput ? tokenInput.value : '';
    }

    function setStatus(container, message, isError) {
        var status = container.querySelector('[data-file-status]');
        if (!status) {
            return;
        }

        status.textContent = message || '';
        status.classList.toggle('text-danger', Boolean(isError));
        status.classList.toggle('text-muted', !isError);
    }

    function refreshList(container) {
        var list = container.querySelector('[data-file-list]');
        var listUrl = container.dataset.filesUrl;

        if (!list || !listUrl) {
            return Promise.resolve();
        }

        return fetch(listUrl, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('Lista datoteka nije dostupna.');
                }

                return response.text();
            })
            .then(function (html) {
                list.innerHTML = html;
            });
    }

    function uploadFiles(container, files) {
        var uploadUrl = container.dataset.uploadUrl;
        var token = getAntiForgeryToken(container);
        var formData = new FormData();

        Array.prototype.forEach.call(files, function (file) {
            formData.append('files', file);
        });

        setStatus(container, 'Upload je u tijeku...', false);

        return fetch(uploadUrl, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': token,
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: formData
        })
            .then(function (response) {
                if (!response.ok) {
                    return response.text().then(function (message) {
                        throw new Error(message || 'Upload nije uspio.');
                    });
                }

                return response.text();
            })
            .then(function (html) {
                container.querySelector('[data-file-list]').innerHTML = html;
                setStatus(container, 'Datoteka je spremljena.', false);
            })
            .catch(function (error) {
                setStatus(container, error.message, true);
            });
    }

    function initFileUpload(container) {
        var input = container.querySelector('[data-file-input]');
        var dropZone = container.querySelector('[data-file-dropzone]');
        var list = container.querySelector('[data-file-list]');

        if (!input || !dropZone || !list) {
            return;
        }

        refreshList(container);

        input.addEventListener('change', function () {
            if (input.files.length > 0) {
                uploadFiles(container, input.files).then(function () {
                    input.value = '';
                });
            }
        });

        dropZone.addEventListener('click', function () {
            input.click();
        });

        dropZone.addEventListener('dragover', function (event) {
            event.preventDefault();
            dropZone.classList.add('border-primary');
        });

        dropZone.addEventListener('dragleave', function () {
            dropZone.classList.remove('border-primary');
        });

        dropZone.addEventListener('drop', function (event) {
            event.preventDefault();
            dropZone.classList.remove('border-primary');

            if (event.dataTransfer.files.length > 0) {
                uploadFiles(container, event.dataTransfer.files);
            }
        });

        list.addEventListener('click', function (event) {
            var button = event.target.closest('[data-file-delete-url]');

            if (!button) {
                return;
            }

            fetch(button.dataset.fileDeleteUrl, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': getAntiForgeryToken(container),
                    'X-Requested-With': 'XMLHttpRequest'
                }
            })
                .then(function (response) {
                    if (!response.ok) {
                        throw new Error('Brisanje datoteke nije uspjelo.');
                    }

                    return response.text();
                })
                .then(function (html) {
                    list.innerHTML = html;
                    setStatus(container, 'Datoteka je obrisana.', false);
                })
                .catch(function (error) {
                    setStatus(container, error.message, true);
                });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('[data-prostor-files]').forEach(initFileUpload);
    });
}());
