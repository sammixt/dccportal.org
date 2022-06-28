/**
 * DataTables Basic
 */

 $(function () {
    'use strict';
  
    var dt_basic_table = $('#expense_list');
    //   dt_date_table = $('.dt-date'),
    //   dt_complex_header_table = $('.dt-complex-header'),
    //   dt_row_grouping_table = $('.dt-row-grouping'),
    //   dt_multilingual_table = $('.dt-multilingual'),
     // assetPath = '../../../app-assets/';
  
    // if ($('body').attr('data-framework') === 'laravel') {
    //   assetPath = $('body').attr('data-asset-path');
    // }
  
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

    $("#spend").on('click', function(e){
        e.preventDefault();
        $("#modal-slide-in").modal({
            show: true,
            backdrop: 'static',
            keyboard: false
        });
    });


    
    var getWalletBalance = function(){
        axios.get(`${url}Finance/GetDeptBalance`)
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
   
    $("#expense-form").on('click','.data-submit',function(e){
        e.preventDefault();
        var model = {
            expenseBy: $("#expenseBy").val(),
            amount: $("#expense-amount").val(),
            paymentDateString : $("#payment-date").val(),
            narration : $("#narration").val(),
           }
           axios.post(`${url}Finance/InsertExpense`, model)
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
  });
  