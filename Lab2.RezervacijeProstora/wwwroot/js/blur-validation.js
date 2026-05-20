(function () {
    const validateElement = (element) => {
        if (!window.jQuery || !jQuery.validator || !element || !element.form) {
            return;
        }

        const form = jQuery(element.form);

        if (!form.data("validator") && jQuery.validator.unobtrusive) {
            jQuery.validator.unobtrusive.parse(form);
        }

        form.validate().element(element);
    };

    const configureValidator = () => {
        if (!window.jQuery || !jQuery.validator) {
            return;
        }

        jQuery.validator.setDefaults({
            ignore: ":hidden:not([data-autocomplete-value]):not([data-date-picker-value])"
        });
    };

    document.addEventListener("DOMContentLoaded", () => {
        configureValidator();

        document.querySelectorAll("form").forEach((form) => {
            form.addEventListener("focusout", (event) => {
                const target = event.target;

                if (!(target instanceof HTMLElement)) {
                    return;
                }

                const autocomplete = target.closest("[data-autocomplete]");
                const datePicker = target.closest("[data-date-picker]");

                if (autocomplete) {
                    window.setTimeout(() => validateElement(autocomplete.querySelector("[data-autocomplete-value]")), 200);
                    return;
                }

                if (datePicker) {
                    validateElement(datePicker.querySelector("[data-date-picker-value]"));
                    return;
                }

                if (target.matches("input, select, textarea")) {
                    validateElement(target);
                }
            }, true);
        });
    });
})();
