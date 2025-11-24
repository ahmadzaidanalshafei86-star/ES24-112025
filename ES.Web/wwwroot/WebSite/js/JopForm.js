// Combined JavaScript for form handling and Select2 initialization
(function ($) {
	$(document).ready(function () {
		// ==================== SELECT2 INITIALIZATION ====================

		// Initialize all select2 dropdowns with the shared class
		$(".select2-dropdown").each(function () {
			$(this).select2({
				placeholder: function () {
					return $(this).data("placeholder");
				},
				width: "100%",
			});
		});

		// ==================== PLACE OF BIRTH TOGGLE ====================

		// Function to toggle state and city dropdowns for Place of Birth
		function togglePlaceOfBirthStateCity() {
			var selectedCountryId = $("#PlaceOfBirthId").val();
			console.log("Selected Country ID:", selectedCountryId);

			if (selectedCountryId == "1") {
				// Jordan
				console.log("Showing state and city");
				$("#stateContainer").show();
				$("#cityContainer").show();
			} else {
				console.log("Hiding state and city");
				$("#stateContainer").hide();
				$("#cityContainer").hide();
				// Clear the selections when hiding
				$("#SelectedStateId").val("").trigger("change");
				$("#SelectedCityId").val("").trigger("change");
			}
		}

		// Check on page load for Place of Birth
		togglePlaceOfBirthStateCity();

		// Check when Place of Birth country selection changes
		$("#PlaceOfBirthId").on("change", function () {
			togglePlaceOfBirthStateCity();
		});

		// Also listen to select2 specific change event
		$("#PlaceOfBirthId").on("select2:select", function () {
			togglePlaceOfBirthStateCity();
		});

		// ==================== STATE/CITY CASCADE ====================

		// State change event handler
		$("#SelectedStateId").on("change", function () {
			var stateId = $(this).val();
			var cityDropdown = $("#SelectedCityId");

			if (stateId) {
				$.get(
					"/JobForm/GetCitiesByCountryAndState",
					{
						stateId: stateId,
					},
					function (data) {
						cityDropdown.empty().append('<option value=""></option>');
						$.each(data, function (i, item) {
							cityDropdown.append(
								'<option value="' + item.value + '">' + item.text + "</option>"
							);
						});
						cityDropdown.trigger("change");
					}
				);
			} else {
				cityDropdown.empty().append('<option value=""></option>');
				cityDropdown.trigger("change");
			}
		});

		// ==================== UNIVERSITY CASCADE ====================

		// Cascade: when university country changes, load its universities
		$("#UniversityCountryId").on("change", function () {
			const countryId = $(this).val();
			const $universitySelect = $("#SelectedUniversityId");
			$universitySelect.empty().append('<option value=""></option>');
			if (countryId) {
				$.ajax({
					url: "/JobForm/GetUniversitiesByCountry",
					data: { countryId },
					success: function (data) {
						$.each(data, function (i, item) {
							$universitySelect.append(new Option(item.text, item.id));
						});
						$universitySelect.trigger("change");
					},
				});
			} else {
				$universitySelect.trigger("change");
			}
		});

		// ==================== ADDRESS STATE/CITY CASCADE ====================

		// Address State change event handler
		$("#AddressStateId").on("change", function () {
			var stateId = $(this).val();
			var cityDropdown = $("#AddressCityId");

			if (stateId) {
				$.get(
					"/JobForm/GetCitiesByCountryAndState",
					{
						stateId: stateId,
					},
					function (data) {
						cityDropdown.empty().append('<option value=""></option>');
						$.each(data, function (i, item) {
							cityDropdown.append(
								'<option value="' + item.value + '">' + item.text + "</option>"
							);
						});
						cityDropdown.trigger("change");
					}
				);
			} else {
				cityDropdown.empty().append('<option value=""></option>');
				cityDropdown.trigger("change");
			}
		});

		// ==================== RESIDENTIAL ADDRESS - HIDE STATE/CITY IF NOT JORDAN ====================

		// Function to toggle state and city for Residential Address
		function toggleResidentialAddressStateCity() {
			var selectedCountryId = $("#AddressCountryId").val();
			console.log(
				"Selected Residential Address Country ID:",
				selectedCountryId
			);

			if (selectedCountryId == "1") {
				// Jordan
				console.log("Showing residential state and city");
				$("#addressStateContainer").show();
				$("#addressCityContainer").show();
			} else {
				console.log("Hiding residential state and city");
				$("#addressStateContainer").hide();
				$("#addressCityContainer").hide();
				// Clear the selections when hiding
				$("#AddressStateId").val("").trigger("change");
				$("#AddressCityId").val("").trigger("change");
			}
		}

		// Check on page load for Residential Address
		toggleResidentialAddressStateCity();

		// Check when Residential Address country selection changes
		$("#AddressCountryId").on("change", function () {
			toggleResidentialAddressStateCity();
		});

		// Also listen to select2 specific change event
		$("#AddressCountryId").on("select2:select", function () {
			toggleResidentialAddressStateCity();
		});
	});
})(jQuery);

// ==================== DOM CONTENT LOADED EVENTS ====================

document.addEventListener("DOMContentLoaded", function () {


	// امنع كل الاقتراحات
	document.querySelectorAll("input").forEach(function (el) {
		el.setAttribute("autocomplete", "nope");
	});



	// ==================== DATE OF BIRTH VALIDATION ====================
	let dateOfBirthInput = document.querySelector('input[name="DateOfBirth"]');
	if (dateOfBirthInput) {
		// اجعل الحقل فارغ افتراضياً
		dateOfBirthInput.value = "";

		// تحديد أقصى تاريخ مسموح به (18 سنة)
		const today = new Date();
		const maxDate = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());
		dateOfBirthInput.setAttribute("max", maxDate.toISOString().split("T")[0]);

		// التحقق عند تغيير القيمة
		dateOfBirthInput.addEventListener("change", function () {
			if (!this.value) return; // لا تفعل أي شيء إذا الحقل فارغ

			const selectedDate = new Date(this.value);
			const todayNow = new Date(); // إعادة تعريف اليوم الحالي عند كل تحقق
			let age = todayNow.getFullYear() - selectedDate.getFullYear();
			const monthDiff = todayNow.getMonth() - selectedDate.getMonth();

			if (monthDiff < 0 || (monthDiff === 0 && todayNow.getDate() < selectedDate.getDate())) {
				age--;
			}

			if (age < 18) {
				alert("You must be at least 18 years old");
				this.value = "";
				this.focus();
			}
		});
	}


	// ==================== NATIONAL NUMBER VALIDATION ====================

	// Validate National Number - exactly 10 digits
	const nationalNumberInput = document.querySelector(
		'input[name="NationalNumber"]'
	);
	if (nationalNumberInput) {
		nationalNumberInput.addEventListener("input", function () {
			// Remove any non-digit characters
			this.value = this.value.replace(/\D/g, "");

			// Limit to 10 digits
			if (this.value.length > 10) {
				this.value = this.value.substring(0, 10);
			}
		});

		nationalNumberInput.addEventListener("blur", function () {
			if (this.value.length > 0 && this.value.length !== 10) {
				alert("National Number must be exactly 10 digits");
				this.value = "";
				this.focus();
			}
		});
	}

	// ==================== COLLAPSIBLE SECTIONS ====================

	// Handle collapsible sections
	document.querySelectorAll(".collapsible").forEach((btn) => {
		btn.addEventListener("click", function () {
			this.classList.toggle("active");
			const content = this.nextElementSibling;
			if (content.style.maxHeight) {
				content.style.maxHeight = null;
			} else {
				content.style.maxHeight = content.scrollHeight + "px";
			}
		});
	});

	// ==================== SCROLL-LINK SECTIONS ====================

	// Handle scroll-link sections (show/hide content)
	var isRtl =
		document.dir === "rtl" ||
		document.documentElement.dir === "rtl" ||
		document.body.dir === "rtl" ||
		getComputedStyle(document.body).direction === "rtl";

	// Show first section by default
	var firstSection = document.querySelector(".content-section");
	var firstArrow = document.querySelector(".arrow-icon");
	if (firstSection) {
		firstSection.style.display = "block";
		if (firstArrow) {
			firstArrow.className = "fa fa-angle-down arrow-icon";
		}
	}

	document.querySelectorAll(".scroll-link").forEach((link) => {
		link.addEventListener("click", function (e) {
			e.preventDefault();
			var targetId = this.getAttribute("data-target");
			var target = document.getElementById(targetId);
			var arrow = this.previousElementSibling;

			// Hide all sections and reset arrows
			document.querySelectorAll(".content-section").forEach((section) => {
				section.style.display = "none";
			});
			document.querySelectorAll(".arrow-icon").forEach((icon) => {
				icon.className =
					"fa fa-angle-" + (isRtl ? "left" : "right") + " arrow-icon";
			});

			// Show selected section and update arrow
			target.style.display = "block";
			arrow.className = "fa fa-angle-down arrow-icon";
		});
	});

	// ==================== DYNAMIC FORM HANDLING ====================

	// Counters for multiple entries
	var counters = {
		language: 0,
		workhistory: 0,
		skill: 0,
	};

	// Initialize counters based on existing items
	document
		.querySelectorAll("#languages-container .item-row")
		.forEach(() => counters.language++);
	document
		.querySelectorAll("#workhistory-container .item-row")
		.forEach(() => counters.workhistory++);
	document
		.querySelectorAll("#skills-container .item-row")
		.forEach(() => counters.skill++);

	// Add new item for Languages, Work History, and Skills
	document.querySelectorAll(".add-item").forEach((button) => {
		button.addEventListener("click", function () {
			var section = this.getAttribute("data-section");
			var template = document.getElementById(section + "-template").innerHTML;
			// container id is plural for some sections (e.g. "languages-container", "skills-container")
			// but for "workhistory" the view uses "workhistory-container" (no extra 's'), so try both.
			var container =
				document.getElementById(section + "s-container") ||
				document.getElementById(section + "-container");
			if (!container) {
				console.warn("Add-item: container not found for section", section);
				return;
			}

			// Replace index placeholder with actual counter
			var newItem = template
				.replace(/\[-1\]/g, "[" + counters[section] + "]")
				.replace(/data-index="-1"/g, 'data-index="' + counters[section] + '"');

			container.insertAdjacentHTML("beforeend", newItem);
			counters[section]++;

			// Reattach event listeners for new remove buttons
			attachRemoveListeners();
		});
	});

	// ==================== ATTACHMENT HANDLING ====================

	// Add new attachment file input
	document
		.getElementById("add-attachment")
		?.addEventListener("click", function () {
			var container = document.getElementById("attachments-container");
			var newRow = `
            <div class="attachment-row mb-3">
                <div class="row">
                    <div class="col-md-10">
                        <input type="file" name="AttachmentFiles" class="form-control" />
                    </div>
                    <div class="col-md-2">
                        <button type="button" class="btn btn-danger btn-sm remove-attachment">
                            <i class="fa fa-trash"></i>
                        </button>
                    </div>
                </div>
            </div>
        `;
			container.insertAdjacentHTML("beforeend", newRow);
			attachAttachmentRemoveListeners();
		});

	// Remove attachment row
	function attachAttachmentRemoveListeners() {
		document.querySelectorAll(".remove-attachment").forEach((button) => {
			button.removeEventListener("click", removeAttachmentHandler);
			button.addEventListener("click", removeAttachmentHandler);
		});
	}

	function removeAttachmentHandler() {
		var row = this.closest(".attachment-row");
		var container = document.getElementById("attachments-container");

		// Don't allow removing if it's the last item
		if (container.querySelectorAll(".attachment-row").length > 1) {
			row.remove();
		} else {
			alert("At Least One Item Required");
		}
	}

	// ==================== REMOVE ITEM HANDLING ====================

	// Remove item for Languages, Work History, and Skills
	function attachRemoveListeners() {
		document.querySelectorAll(".remove-item").forEach((button) => {
			button.removeEventListener("click", removeItemHandler);
			button.addEventListener("click", removeItemHandler);
		});
	}

	function removeItemHandler() {
		var item = this.closest(".item-row");
		var container = item.parentElement;

		// Don't allow removing if it's the last item
		if (container.querySelectorAll(".item-row").length > 1) {
			item.remove();
			reindexItems(container);
		} else {
			alert("At Least One Item Required");
		}
	}

	// ==================== REINDEXING ====================

	// Reindex items after removal
	function reindexItems(container) {
		var items = container.querySelectorAll(".item-row");
		items.forEach((item, index) => {
			item.setAttribute("data-index", index);

			// Update all input names
			item.querySelectorAll("input, select, textarea").forEach((input) => {
				var name = input.getAttribute("name");
				if (name) {
					var newName = name.replace(/\[\d+\]/, "[" + index + "]");
					input.setAttribute("name", newName);
				}
			});
		});
	}

	// ==================== INITIALIZE EVENT LISTENERS ====================

	// Initial attachment of event listeners
	attachRemoveListeners();
	attachAttachmentRemoveListeners();

	// ==================== DISABLE SUBMIT BUTTON ON SUBMIT ====================
	var employeeForm = document.getElementById("employeeForm");
	if (employeeForm) {
		employeeForm.addEventListener("submit", function () {
			var submitBtn = this.querySelector('button[type="submit"]');
			if (submitBtn) {
				submitBtn.disabled = true;
				submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>جاري الإرسال...';
			}
		});
	}
});
