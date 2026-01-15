const baseUrl = window.appBaseUrl || '/';
$(document).ready(function () {
    var languageUrl = '';

    // Determine the language URL based on the current language
    if (currentLanguage === 'ar-JO') {
        languageUrl = "https://cdn.datatables.net/plug-ins/1.10.21/i18n/Arabic.json";
    } else if (currentLanguage === 'en-JO') {
        languageUrl = "https://cdn.datatables.net/plug-ins/1.10.21/i18n/English.json";
    } else {
        // Default to Arabic if not recognized
        languageUrl = "https://cdn.datatables.net/plug-ins/1.10.21/i18n/Arabic.json";
    }

    // Initialize DataTable
    var table = $('#table').DataTable({
        "pageLength": 50,
        "lengthMenu": [[50, 75, 100, 120], [50, 75, 100, 120]],
        "language": {
            "url": languageUrl
        },
        "order": [[1, "asc"]]
    });

    // Toggle PurchaseOrder status
    $(document).on('click', '.toggle-status', function () {
        const purchaseOrderId = $(this).data('purchaseOrder-id');
        togglePurchaseOrderstatus(purchaseOrderId, this);
    });

    // Store original value and handle input changes
    $('.order-input').each(function () {
        const $input = $(this);
        $input.data('original-value', $input.val());
    });

    // Show/hide save icon on change
    $('.order-input').on('input', function () {
        const $input = $(this);
        const $saveIcon = $input.next('.save-order');
        const currentValue = $input.val();
        const originalValue = $input.data('original-value');

        if (currentValue !== originalValue && currentValue && !isNaN(currentValue)) {
            $saveIcon.fadeIn(200);
        } else {
            $saveIcon.fadeOut(200);
        }
    });

    // Enter key handler
    $('.order-input').on('keypress', async function (e) {
        if (e.which === 13) {
            e.preventDefault();
            const $input = $(this);
            const purchaseOrderId = $input.data('purchaseOrder-id');
            const newOrder = $input.val();

            if (!newOrder || isNaN(newOrder)) {
                $input.addClass('is-invalid');
                return;
            }

            await updateOrder($input, purchaseOrderId, newOrder);
        }
    });

    // Save icon click handler
    $('.save-order').on('click', async function () {
        const $saveIcon = $(this);
        const purchaseOrderId = $saveIcon.data('purchaseOrder-id');
        const $input = $saveIcon.prev('.order-input');
        const newOrder = $input.val();

        if (!newOrder || isNaN(newOrder)) {
            $input.addClass('is-invalid');
            return;
        }

        await updateOrder($input, purchaseOrderId, newOrder);
    });
});

async function updateOrder($input, purchaseOrderId, newOrder) {
    try {
        $input.prop('disabled', true);
        const $saveIcon = $input.next('.save-order');
        $saveIcon.addClass('is-loading');

        const updateUrl = `${baseUrl}EsAdmin/PurchaseOrders/UpdateOrder?id=${purchaseOrderId}&order=${newOrder}`;

        const response = await fetch(updateUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            }
        });

        if (response.status === 200) {
            $input.removeClass('is-loading').addClass('is-valid');
            $saveIcon.removeClass('is-loading').fadeOut(200);
            $input.data('original-value', newOrder); // Update original value after success

            Swal.fire({
                icon: 'success',
                title: currentLanguage === 'ar-JO' ? 'تم بنجاح!' : 'Done successfully!',
                showConfirmButton: true
            });

            setTimeout(() => $input.removeClass('is-valid'), 1000);
        } else if (response.status === 403) {
            Swal.fire({
                icon: 'error',
                title: currentLanguage === 'ar-JO' ? 'ليس لديك إذن لتعديل هذا العطاء!' : 'You don\'t have permission to edit this PurchaseOrder!',
                showConfirmButton: true
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: currentLanguage === 'ar-JO' ? 'لا يمكن تحديث الترتيب' : 'Can\'t update order',
                showConfirmButton: true
            });
        }
    } catch (error) {
        console.error('Error:', error);
        Swal.fire({
            icon: 'error',
            title: currentLanguage === 'ar-JO' ? 'حدث خطأ' : 'An error occurred',
            showConfirmButton: true
        });
    } finally {
        $input.prop('disabled', false);
        $input.next('.save-order').removeClass('is-loading');
    }
}

async function togglePurchaseOrderstatus(purchaseOrderId, element) {
    try {
        const ToggleUrl = `${baseUrl}EsAdmin/PurchaseOrders/ToggleStatus?id=${purchaseOrderId}`;
        const response = await fetch(ToggleUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            }
        });

        if (response.status === 200) {
            const row = $(element).closest('tr');
            const badgeSpan = row.find('td span.badge');

            if (badgeSpan.length) {
                const isActive = badgeSpan.text().trim() === 'Published';
                badgeSpan.hide().text(isActive ? 'Not published' : 'Published').fadeIn(200);
                badgeSpan.toggleClass('bg-success bg-danger');
            }

            Swal.fire({
                icon: 'success',
                title: currentLanguage === 'ar-JO' ? 'تم بنجاح!' : 'Done successfully!',
                showConfirmButton: true
            });

            setTimeout(function () {
                location.reload();
            }, 2000);
        } else if (response.status === 403) {
            Swal.fire({
                icon: 'error',
                title: currentLanguage === 'ar-JO' ? 'ليس لديك إذن لتعديل حالة هذا العطاء!' : 'You don\'t have permission to edit this PurchaseOrder status!',
                showConfirmButton: true
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: currentLanguage === 'ar-JO' ? 'لا يمكن نشر العطاء' : 'Can\'t publish PurchaseOrder',
                showConfirmButton: true
            });
        }
    } catch (error) {
        console.error('Error:', error);
        Swal.fire({
            icon: 'error',
            title: currentLanguage === 'ar-JO' ? 'حدث خطأ' : 'An error occurred',
            showConfirmButton: true
        });
    }
}

async function deletePurchaseOrder(purchaseOrderId, element) {
    console.log('deletePurchaseOrder called for ID:', purchaseOrderId);

    const confirmDeletion = await Swal.fire({
        icon: 'warning',
        title: currentLanguage === 'ar-JO' ? 'هل أنت متأكد من أنك تريد حذف هذا العطاء؟' : 'Are you sure you want to delete this PurchaseOrder?',
        showCancelButton: true,
        confirmButtonText: currentLanguage === 'ar-JO' ? 'نعم' : 'Yes',
        cancelButtonText: currentLanguage === 'ar-JO' ? 'لا' : 'No'
    });

    if (!confirmDeletion.isConfirmed) return;

    try {
        const DeleteUrl = `${baseUrl}EsAdmin/PurchaseOrders/Delete?id=${purchaseOrderId}`;
        const response = await fetch(DeleteUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        console.log('Response:', response);

        if (response.ok) {
            console.log('PurchaseOrder deleted successfully.');
            const row = $(element).closest('tr');
            row.fadeOut(500, () => row.remove());

            Swal.fire({
                icon: 'success',
                title: currentLanguage === 'ar-JO' ? 'تم حذف العطاء بنجاح!' : 'PurchaseOrder deleted successfully!',
                showConfirmButton: true
            });
        } else if (response.status == 403) {
            const responseText = await response.text();
            console.error('Failed to delete PurchaseOrder:', responseText);

            Swal.fire({
                icon: 'error',
                title: currentLanguage === 'ar-JO' ? 'ليس لديك إذن لحذف هذا العطاء!' : 'You don\'t have permission to delete this PurchaseOrder!',
                showConfirmButton: true
            });
        }
    } catch (error) {
        console.error('Error deleting PurchaseOrder', error);
        Swal.fire({
            icon: 'error',
            title: currentLanguage === 'ar-JO' ? 'حدث خطأ أثناء حذف العطاء' : 'An error occurred while deleting the PurchaseOrder.',
            showConfirmButton: true
        });
    }
}
