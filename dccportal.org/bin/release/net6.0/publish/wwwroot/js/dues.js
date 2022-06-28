$(function(){
    'use strict';

    var dt_basic_table = $('#dues_list');
    if (dt_basic_table.length) {
        dt_basic_table.DataTable({
        
            language: {
                paginate: {
                    
                    previous: '&nbsp;',
                    next: '&nbsp;'
                }
            }
        })
    };

    $("#dues_list tbody").on('click','.details', function(e){
        e.preventDefault();
           var setDuesIdString = $(this).attr("data-id"); 
            var duesName = $(this).attr("data-name")
            var duesDesc= $(this).attr("data-desc");
            var duesType = $(this).attr("data-type");
            var amount = $(this).attr("data-amount")
    
            $("#edit-payment-name").val(duesName),
            $("#edit-payment-description").val(duesDesc),
            $("#edit-payment-type").val(duesType),
            $("#edit-amount").val(amount)
            $("#setDuesIdString").val(setDuesIdString)
           
            $("#modals-slide-in").modal({
                show: true,
                backdrop: 'static',
                keyboard: false
            });
            
        
    });

    $("#new-dues-form").on('click','.btn-submit',function(e){
        e.preventDefault();
        var model = {
            duesName : $("#payment-name").val(),
            duesDesc : $("#payment-description").val(),
            duesType: $("#payment-type").val(),
            amount : $("#amount").val()
           }
           axios.post(`${url}Finance/AddPaymentType`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    $(".form-group").find("input[type=text], textarea").val("");
                    window.location.reload();
                } else {
                    
                    toastr['error'](`👋 ${response.data.message}`, 'Error!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                }
           })
           .catch(function (error) {
            processErrors(error)
           });
    });

    $("#edit-dues-form").on('click','.data-submit',function(e){
        e.preventDefault();
        var model = {
            duesName : $("#edit-payment-name").val(),
            duesDesc : $("#edit-payment-description").val(),
            duesType: $("#edit-payment-type").val(),
            amount : $("#edit-amount").val(),
            setDuesIdString: $("#setDuesIdString").val()
           }
           axios.put(`${url}Finance/EditPaymentType`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    $('#modal-slide-in').modal('hide')
                    window.location.reload();
                } else {
                    
                    toastr['error'](`👋 ${response.data.message}`, 'Error!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                }
           })
           .catch(function (error) {
            processErrors(error)
           });
    });

    $("#dues_list tbody").on('click', 'a.delete-record', function (e) {
        e.preventDefault();
        
        var id = $(this).attr("data-id");
        //alert(id);
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            customClass: {
                confirmButton: 'btn btn-primary',
                cancelButton: 'btn btn-outline-danger ml-1'
            },
            buttonsStyling: false
        }).then(function (result) {
            if (result.value) {
                axios.delete(`${url}Finance/DeletePaymentType?setDuesIdString=${id}`)
                    .then(response => {
                        Swal.fire({
                            icon: 'success',
                            title: 'Deleted!',
                            text: `${response.data.message}`,
                            customClass: {
                                confirmButton: 'btn btn-success'
                            }
                        }).then(function (result_deleted) {
                            if (result_deleted.value) {
                                window.location.reload();
                            }
                        });
                    })
                    .catch(function (error) {
                        if (error.response) {
                            toastr['error'](error.response.data.message, 'Error!', {
                                closeButton: true,
                                tapToDismiss: false,
                                rtl: false
                            });
                        }
    
    
                    });
    
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    title: 'Cancelled',
                    text: 'Payment type is intact 😀',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-success'
                    }
                });
            }
        });
    
    });
})