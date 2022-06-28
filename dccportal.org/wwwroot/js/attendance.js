$(function(){
    'use strict';

    var dt_basic_table = $('#unit_list');
   
  
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

    $("#attendance_checker").on('change','.mark',function(){
        var idr = getUrlParameter('_idstr');
        var ischeck = $(this).is(":checked")
        var id = $(this).attr("data-id");
        
        var model = {
            status:ischeck,
            memberId:id,
            getAttandanceIdString:idr
        }
        // alert(ischeck);
        // alert(id);
        // alert(idr)
        axios.post(`${url}Attendance/UpdateStatus`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    if(ischeck){
                        $(`.${id}`).text("Present")
                    }else{
                        $(`.${id}`).text("Absent")
                    }
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
           })
    })

    $("#new-attendance-form").on('click','.btn-submit',function(e){
        e.preventDefault();
        var model = {
            setAttendanceDate : $("#attendance-date").val(),
            departmentGroup : $("#vertical-group option:selected").val(),
           }
           axios.post(`${url}Attendance/ValidateDate`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 Attendance Initiated`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    $(".form-group").find("input[type=text], textarea").val("");
                    window.location.href = `${url}Attendance/Index?_idstr=${response.data.message}`
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

    $("#new-attendance-form").on('click','.btn-search',function(e){
        e.preventDefault();
        var model = {
            setAttendanceDate : $("#attendance-date").val(),
            departmentGroup : $("#vertical-group option:selected").val(),
           }
           axios.post(`${url}Attendance/SearchDate`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 Attendance Record Found`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    $(".form-group").find("input[type=text], textarea").val("");
                    window.location.href = `${url}Attendance/Index?_idstr=${response.data.message}`
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
})