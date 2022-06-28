/**
 * DataTables Basic
 */

 $(function () {
    'use strict';
  
    var dt_basic_table = $('#dues_list');
    
    // DataTable with buttons
    // --------------------------------------------------------------------
  
    if (dt_basic_table.length) {
        dt_basic_table.DataTable({
        
            language: {
                paginate: {
                    
                    previous: '&nbsp;',
                    next: '&nbsp;'
                }
            }
        })
    }
  

    $("#pay").on('click', function(e){
        e.preventDefault();
        $("#modal-slide-in").modal({
            show: true,
            backdrop: 'static',
            keyboard: false
        });
    });

    $("#topup").on('click', function(e){
        e.preventDefault();
        $("#modal-slide-in-topup").modal({
            show: true,
            backdrop: 'static',
            keyboard: false
        });
    });
    
    var getWalletBalance = function(){
        var memberId = getUrlParameter('_memberId');
        axios.get(`${url}Finance/GetWalletBalance?id=${memberId}`)
        .then(response => {
            var d = response.data;
            $("#amount").text(d.amount)
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
    }
    
    getWalletBalance();

    $("#vertical-payment-type").on('change',function () {
        var value = $("#vertical-payment-type option:selected").val();
        if (value == 4) {
            $("#MonthDiv").css('display', 'block');
            $("#Year").css('display', 'block');
        } else {
            $("#MonthDiv").css('display', 'none');
            $("#Year").css('display', 'none');
        }
        axios.get(`${url}Finance/getAmount?DuesId=${value}`)
        .then(response => {
            var d = response.data;
            $("#dues-amount").val(d.amount)
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
    })
   

    $("#top-up-form").on('click','.data-submit',function(e){
        e.preventDefault();
        var model = {
            memberId : $("#member").val(),
            amount : $("#topup-amount").val(),
            description: $("#topup-narration").val(),
            transactionType : "Credit",
           }
           axios.post(`${url}Finance/InsertToUpWallet`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    $('#modal-slide-in-topup').modal('hide')
                    getWalletBalance();
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

    $("#dues-payment-form").on('click','.data-submit',function(e){
        e.preventDefault();
        var model = {
            duesId :$("#vertical-payment-type").val(),
            month: $("#Month").val(),
            year: $("#Year option:selected").val(),
            amount: $("#dues-amount").val(),
            memberId: $("#memberId").val(),
            paymentDateString : $("#payment-date").val(),
            paymentMethod : $("#vertical-payment-method option:selected").val(),
            narration : $("#narration").val(),
           }
           axios.post(`${url}Finance/InsertDuesPayment`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    $('#modal-slide-in-topup').modal('hide')
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
  });
  