(function () {
    const monthNames = [
        "Siječanj", "Veljača", "Ožujak", "Travanj", "Svibanj", "Lipanj",
        "Srpanj", "Kolovoz", "Rujan", "Listopad", "Studeni", "Prosinac"
    ];

    const dayNames = ["Pon", "Uto", "Sri", "Čet", "Pet", "Sub", "Ned"];

    const pad = (value) => value.toString().padStart(2, "0");

    const toDisplay = (date) => {
        return `${pad(date.getDate())}.${pad(date.getMonth() + 1)}.${date.getFullYear()}. ${pad(date.getHours())}:${pad(date.getMinutes())}`;
    };

    const toSubmit = (date) => {
        return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
    };

    const parseDisplay = (value) => {
        const match = value.trim().match(/^(\d{1,2})\.(\d{1,2})\.(\d{4})\.?\s+(\d{1,2}):(\d{2})$/);

        if (!match) {
            return null;
        }

        const [, day, month, year, hour, minute] = match.map(Number);
        const date = new Date(year, month - 1, day, hour, minute);

        if (date.getFullYear() !== year || date.getMonth() !== month - 1 || date.getDate() !== day) {
            return null;
        }

        return date;
    };

    const parseSubmit = (value) => {
        const date = new Date(value);
        return Number.isNaN(date.getTime()) ? new Date() : date;
    };

    const setDate = (control, date) => {
        control.selectedDate = date;
        control.querySelector("[data-date-picker-display]").value = toDisplay(date);
        control.querySelector("[data-date-picker-value]").value = toSubmit(date);
        control.querySelector("[data-date-picker-value]").dispatchEvent(new Event("change", { bubbles: true }));
    };

    const firstGridDate = (year, month) => {
        const first = new Date(year, month, 1);
        const day = first.getDay() === 0 ? 7 : first.getDay();
        const start = new Date(first);
        start.setDate(first.getDate() - day + 1);
        return start;
    };

    const render = (control) => {
        const panel = control.querySelector("[data-date-picker-panel]");
        const date = control.visibleDate || control.selectedDate;
        const year = date.getFullYear();
        const month = date.getMonth();
        const start = firstGridDate(year, month);
        const time = `${pad(control.selectedDate.getHours())}:${pad(control.selectedDate.getMinutes())}`;

        panel.innerHTML = "";

        const header = document.createElement("div");
        header.className = "date-picker-header";

        const previous = document.createElement("button");
        previous.type = "button";
        previous.textContent = "‹";
        previous.addEventListener("click", () => {
            control.visibleDate = new Date(year, month - 1, 1);
            render(control);
        });

        const title = document.createElement("strong");
        title.textContent = `${monthNames[month]} ${year}`;

        const next = document.createElement("button");
        next.type = "button";
        next.textContent = "›";
        next.addEventListener("click", () => {
            control.visibleDate = new Date(year, month + 1, 1);
            render(control);
        });

        header.append(previous, title, next);
        panel.appendChild(header);

        const grid = document.createElement("div");
        grid.className = "date-picker-grid";

        dayNames.forEach((name) => {
            const dayName = document.createElement("span");
            dayName.className = "date-picker-day-name";
            dayName.textContent = name;
            grid.appendChild(dayName);
        });

        for (let i = 0; i < 42; i += 1) {
            const current = new Date(start);
            current.setDate(start.getDate() + i);

            const button = document.createElement("button");
            button.type = "button";
            button.className = "date-picker-day";
            button.textContent = current.getDate().toString();

            if (current.getMonth() !== month) {
                button.classList.add("muted");
            }

            if (
                current.getFullYear() === control.selectedDate.getFullYear() &&
                current.getMonth() === control.selectedDate.getMonth() &&
                current.getDate() === control.selectedDate.getDate()
            ) {
                button.classList.add("selected");
            }

            button.addEventListener("click", () => {
                const selected = new Date(current);
                selected.setHours(control.selectedDate.getHours(), control.selectedDate.getMinutes(), 0, 0);
                setDate(control, selected);
                control.visibleDate = selected;
                render(control);
            });

            grid.appendChild(button);
        }

        panel.appendChild(grid);

        const footer = document.createElement("div");
        footer.className = "date-picker-footer";

        const timeInput = document.createElement("input");
        timeInput.className = "form-control";
        timeInput.type = "text";
        timeInput.value = time;
        timeInput.placeholder = "HH:mm";
        timeInput.addEventListener("change", () => {
            const match = timeInput.value.trim().match(/^(\d{1,2}):(\d{2})$/);

            if (!match) {
                timeInput.value = `${pad(control.selectedDate.getHours())}:${pad(control.selectedDate.getMinutes())}`;
                return;
            }

            const hour = Number(match[1]);
            const minute = Number(match[2]);

            if (hour > 23 || minute > 59) {
                timeInput.value = `${pad(control.selectedDate.getHours())}:${pad(control.selectedDate.getMinutes())}`;
                return;
            }

            const selected = new Date(control.selectedDate);
            selected.setHours(hour, minute, 0, 0);
            setDate(control, selected);
        });

        const close = document.createElement("button");
        close.type = "button";
        close.className = "btn btn-primary";
        close.textContent = "Gotovo";
        close.addEventListener("click", () => panel.classList.remove("open"));

        footer.append(timeInput, close);
        panel.appendChild(footer);
    };

    document.querySelectorAll("[data-date-picker]").forEach((control) => {
        const displayInput = control.querySelector("[data-date-picker-display]");
        const valueInput = control.querySelector("[data-date-picker-value]");
        const panel = control.querySelector("[data-date-picker-panel]");
        const toggle = control.querySelector("[data-date-picker-toggle]");

        control.selectedDate = parseSubmit(valueInput.value);
        control.visibleDate = new Date(control.selectedDate);

        toggle.addEventListener("click", () => {
            panel.classList.toggle("open");
            render(control);
        });

        displayInput.addEventListener("change", () => {
            const parsed = parseDisplay(displayInput.value);

            if (parsed) {
                setDate(control, parsed);
                control.visibleDate = new Date(parsed);
            } else {
                displayInput.value = toDisplay(control.selectedDate);
            }
        });

        document.addEventListener("click", (event) => {
            if (!control.contains(event.target)) {
                panel.classList.remove("open");
            }
        });
    });
})();
