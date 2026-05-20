(function () {
    const debounce = (callback, delay) => {
        let timeoutId;

        return (...args) => {
            window.clearTimeout(timeoutId);
            timeoutId = window.setTimeout(() => callback(...args), delay);
        };
    };

    const runSearch = async (form) => {
        const input = form.querySelector("input[name='q']");
        const status = form.querySelector("[data-ajax-search-status]");
        const target = document.querySelector(form.dataset.resultsTarget);
        const url = new URL(form.dataset.searchUrl, window.location.origin);

        url.searchParams.set("q", input.value);

        if (status) {
            status.textContent = "Pretraživanje...";
        }

        try {
            const response = await fetch(url, {
                headers: { "X-Requested-With": "XMLHttpRequest" }
            });

            if (!response.ok) {
                throw new Error("Search request failed");
            }

            target.innerHTML = await response.text();

            if (status) {
                status.textContent = input.value.trim()
                    ? "Rezultati su osvježeni."
                    : "";
            }
        } catch {
            if (status) {
                status.textContent = "Pretraga trenutno nije uspjela.";
            }
        }
    };

    document.querySelectorAll("[data-ajax-search]").forEach((form) => {
        const input = form.querySelector("input[name='q']");
        const debouncedSearch = debounce(() => runSearch(form), 250);

        input.addEventListener("input", debouncedSearch);
        form.addEventListener("submit", (event) => {
            event.preventDefault();
            runSearch(form);
        });
    });
})();
