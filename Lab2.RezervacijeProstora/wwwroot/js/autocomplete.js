(function () {
    const debounce = (callback, delay) => {
        let timeoutId;

        return (...args) => {
            window.clearTimeout(timeoutId);
            timeoutId = window.setTimeout(() => callback(...args), delay);
        };
    };

    const closeMenu = (control) => {
        control.querySelector("[data-autocomplete-menu]").innerHTML = "";
    };

    const selectItem = (control, item) => {
        const textInput = control.querySelector("[data-autocomplete-text]");
        const valueInput = control.querySelector("[data-autocomplete-value]");

        textInput.value = item.label;
        valueInput.value = item.id;
        valueInput.dispatchEvent(new Event("change", { bubbles: true }));
        closeMenu(control);
    };

    const autoSelectWhenUnambiguous = (control) => {
        const textInput = control.querySelector("[data-autocomplete-text]");
        const valueInput = control.querySelector("[data-autocomplete-value]");
        const typed = textInput.value.trim().toLowerCase();
        const items = control.autocompleteItems || [];

        if (valueInput.value || !typed || items.length === 0) {
            return;
        }

        const exactMatch = items.find((item) => item.label.toLowerCase() === typed);

        if (exactMatch) {
            selectItem(control, exactMatch);
            return;
        }

        if (items.length === 1) {
            selectItem(control, items[0]);
        }
    };

    const renderItems = (control, items) => {
        const menu = control.querySelector("[data-autocomplete-menu]");

        control.autocompleteItems = items;
        menu.innerHTML = "";

        items.forEach((item) => {
            const button = document.createElement("button");
            button.type = "button";
            button.className = "autocomplete-item";
            button.textContent = item.label;

            button.addEventListener("click", () => {
                selectItem(control, item);
            });

            menu.appendChild(button);
        });
    };

    const search = async (control) => {
        const textInput = control.querySelector("[data-autocomplete-text]");
        const valueInput = control.querySelector("[data-autocomplete-value]");
        const url = new URL(control.dataset.autocompleteUrl, window.location.origin);

        valueInput.value = "";
        url.searchParams.set("term", textInput.value);

        if (textInput.value.trim().length < 1) {
            control.autocompleteItems = [];
            closeMenu(control);
            return;
        }

        const response = await fetch(url, {
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        if (!response.ok) {
            control.autocompleteItems = [];
            closeMenu(control);
            return;
        }

        renderItems(control, await response.json());
    };

    document.querySelectorAll("[data-autocomplete]").forEach((control) => {
        const textInput = control.querySelector("[data-autocomplete-text]");
        const debouncedSearch = debounce(() => search(control), 250);

        textInput.addEventListener("input", debouncedSearch);
        textInput.addEventListener("blur", () => {
            window.setTimeout(() => {
                autoSelectWhenUnambiguous(control);
                closeMenu(control);
            }, 150);
        });
        textInput.addEventListener("focus", () => {
            if (textInput.value.trim()) {
                search(control);
            }
        });
    });
})();
