// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function confirmAndDelete(deleteUrl, successRedirectUrl, itemName) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "Cancel",
        confirmButtonClass: "btn btn-primary",
        cancelButtonClass: "btn btn-danger ml-1",
        buttonsStyling: false
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: deleteUrl,
                type: 'POST',
                success: function () {
                    Swal.fire({
                        icon: 'success',
                        title: 'Deleted!',
                        text: `${itemName} has been deleted.`,
                        confirmButtonClass: "btn btn-success"
                    }).then(() => {
                        // Redirect after successful deletion
                        window.location.href = successRedirectUrl;
                    });
                },
                error: function (xhr) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong! ' + xhr.responseText,
                        confirmButtonClass: "btn btn-danger"
                    });
                }
            });
        }
    });
}


function showSubmitConfirmation(addUrl, itemName, formId, confirmCallback) {
    Swal.fire({
        title: "Are you sure?",
        text: `You want to add this ${itemName}?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
    }).then((result) => {
        if (result.isConfirmed)
        {
            var formData = $(formId).serialize();
            $.ajax({
                url: addUrl,
                type: 'POST',
                data: formData,
                success: function () {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success!',
                        text: `${itemName} has been successfully added!`,
                        confirmButtonClass: "btn btn-success"
                    }).then(() =>
                    {
                        if (typeof confirmCallback === 'function')
                        {
                            confirmCallback();
                        }
                    });
                },
                error: function (xhr) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong! ' + xhr.responseText,
                        confirmButtonClass: "btn btn-danger"
                    });
                }
            });
        }
    });
}