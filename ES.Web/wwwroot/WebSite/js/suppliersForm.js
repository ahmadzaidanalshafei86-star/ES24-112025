document.addEventListener("DOMContentLoaded", function () {
	// Handle UserType dropdown change to show/hide RegistrationNumber
	const userTypeSelect = document.querySelector('[name="UserType"]');
	if (userTypeSelect) {
		userTypeSelect.addEventListener("change", toggleRegistrationNumber);
		// Trigger on page load if value exists
		if (userTypeSelect.value) {
			toggleRegistrationNumber();
		}
	}

	// Handle materials search
	const materialsSearchInput = document.getElementById("materialsSearchInput");
	if (materialsSearchInput) {
		materialsSearchInput.addEventListener("keyup", filterMaterials);
	}

	// Handle materials checkbox selection
	const materialCheckboxes = document.querySelectorAll(".material-checkbox");
	materialCheckboxes.forEach((checkbox) => {
		checkbox.addEventListener("change", updateMaterialSelection);
	});
});

/**
 * Toggle visibility of RegistrationNumber field based on UserType selection
 */
function toggleRegistrationNumber() {
	const userTypeSelect = document.querySelector('[name="UserType"]');
	const registrationNumberContainer = document.getElementById(
		"registrationNumberContainer"
	);

	if (!userTypeSelect || !registrationNumberContainer) {
		return;
	}

	const userType = userTypeSelect.value;

	if (userType === "Company") {
		// Show with animation
		registrationNumberContainer.style.display = "block";
		registrationNumberContainer.classList.add("fade-in");
	} else {
		// Hide and clear the field
		registrationNumberContainer.style.display = "none";
		registrationNumberContainer.classList.remove("fade-in");

		// Clear the input value when hidden
		const registrationInput = registrationNumberContainer.querySelector(
			'[name="RegistrationNumber"]'
		);
		if (registrationInput) {
			registrationInput.value = "";
		}
	}
}

/**
 * Filter materials based on search input
 */
function filterMaterials() {
	const searchInput = document.getElementById("materialsSearchInput");
	const searchTerm = searchInput.value.toLowerCase();
	const materialItems = document.querySelectorAll(".material-item");

	materialItems.forEach((item) => {
		const label = item.querySelector(".material-label");
		const materialName = label.textContent.toLowerCase();

		if (materialName.includes(searchTerm)) {
			item.style.display = "block";
			item.classList.add("fade-in");
		} else {
			item.style.display = "none";
			item.classList.remove("fade-in");
		}
	});
}

/**
 * Update material selection (for tracking checked materials)
 */
function updateMaterialSelection() {
	const checkbox = this;
	const materialContainer = checkbox.closest(".material-item");

	if (checkbox.checked) {
		materialContainer.classList.add("selected");
	} else {
		materialContainer.classList.remove("selected");
	}
}

/**
 * Validate form before submission
 */
function validateSupplierForm() {
	const userType = document.querySelector('[name="UserType"]').value;
	const companyName = document
		.querySelector('[name="CompanyName"]')
		.value.trim();
	const companySector = document.querySelector('[name="CompanySector"]').value;
	const phoneNumber = document
		.querySelector('[name="PhoneNumber"]')
		.value.trim();
	const emailAddress = document
		.querySelector('[name="EmailAddress"]')
		.value.trim();
	const commercialRegister = document.querySelector(
		'[name="CommercialRegister"]'
	);

	let isValid = true;

	// Validate required fields
	if (!userType) {
		showFieldError("User Type is required");
		isValid = false;
	}

	if (!companyName) {
		showFieldError("Company Name is required");
		isValid = false;
	}

	if (!companySector) {
		showFieldError("Company Sector is required");
		isValid = false;
	}

	if (!phoneNumber) {
		showFieldError("Phone Number is required");
		isValid = false;
	}

	if (!emailAddress) {
		showFieldError("Email Address is required");
		isValid = false;
	}

	// Validate email format
	if (emailAddress && !isValidEmail(emailAddress)) {
		showFieldError("Please enter a valid email address");
		isValid = false;
	}

	// Validate registration number if Company is selected
	if (userType === "Company") {
		const registrationNumber = document
			.querySelector('[name="RegistrationNumber"]')
			.value.trim();
		if (!registrationNumber) {
			showFieldError("Registration Number is required for Companies");
			isValid = false;
		}
	}

	// Validate file upload
	if (!commercialRegister.files || commercialRegister.files.length === 0) {
		showFieldError("Commercial Register document is required");
		isValid = false;
	}

	return isValid;
}

/**
 * Check if email is valid
 */
function isValidEmail(email) {
	const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
	return emailRegex.test(email);
}

/**
 * Show field error message (you can customize this)
 */
function showFieldError(message) {
	// You can implement a toast/alert system here
	console.warn("Form Validation Error:", message);
}

// Add CSS for fade-in animation and checkbox styling - INJECT IMMEDIATELY
const style = document.createElement("style");
style.setAttribute("type", "text/css");
style.textContent = `
    .fade-in {
        animation: fadeIn 0.3s ease-in;
    }

    @keyframes fadeIn {
        from {
            opacity: 0;
            transform: translateY(-10px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    /* Checkbox styling - HIGH PRIORITY */
    .form-check-input {
        display: inline-block !important;
        width: 1.25rem !important;
        height: 1.25rem !important;
        padding: 0 !important;
        margin: 0.125rem 0.5rem 0.125rem 0 !important;
        vertical-align: middle !important;
        background-color: #fff !important;
        border: 2px solid #dee2e6 !important;
        border-radius: 0.25rem !important;
        appearance: none !important;
        -webkit-appearance: none !important;
        -moz-appearance: none !important;
        cursor: pointer !important;
        transition: background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out !important;
        flex-shrink: 0 !important;
    }

    .form-check-input:checked {
        background-color: #0d6efd !important;
        border-color: #0d6efd !important;
        background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 20 20'%3e%3cpath fill='none' stroke='%23fff' stroke-linecap='round' stroke-linejoin='round' stroke-width='3' d='M6 10l3 3l6-6'/%3e%3c/svg%3e") !important;
        background-repeat: no-repeat !important;
        background-position: center !important;
        background-size: contain !important;
    }

    .form-check-input:hover {
        border-color: #0d6efd !important;
        background-color: #e7f1ff !important;
    }

    .form-check-input:focus {
        border-color: #0d6efd !important;
        outline: 0 !important;
        box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25) !important;
    }

    .form-check-input:disabled {
        pointer-events: none !important;
        filter: none !important;
        opacity: 0.5 !important;
    }

    .material-label {
        cursor: pointer !important;
        user-select: none !important;
        margin-bottom: 0 !important;
        display: inline !important;
        font-weight: 500 !important;
    }

    .material-label:hover {
        color: #0d6efd !important;
    }

    .form-check {
        display: flex !important;
        align-items: center !important;
        gap: 0.5rem !important;
    }
`;

// Inject at the very beginning of head
if (document.head) {
	document.head.insertBefore(style, document.head.firstChild);
} else {
	document.addEventListener("DOMContentLoaded", function () {
		document.head.insertBefore(style, document.head.firstChild);
	});
}
