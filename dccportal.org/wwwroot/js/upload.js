$(function(){
    'use strict';

    $('#upload-user-form').on('click','.data-submit',function (e) {
        e.preventDefault();
        var deptId = $('#vertical-department option:selected').val();
        var _files = $(`#customFile1`)[0].files[0];
        var formData = new FormData();
        formData.append('excelFile', _files);
        formData.append('deptId', deptId);
    
        axios.post(`${url}Upload/UploadUser`, formData)
            .then(response => {
                toastr['success'](`👋 ${response.data}`, 'Success!', {
                    closeButton: true,
                    tapToDismiss: true,
                    rtl: false
                });
                const uploadedUser = response.data;
                if(uploadedUser.successful.length > 0)
                {
                    var tbody = $("#successful");
                    $.each(uploadedUser.successful, function(d,v){
                        tbody.append(`<tr>
                        <td>${v.firstName}</td>
                        <td>${v.lastName}</td>
                        <td>${v.gender}</td></tr>
                        `);
                        console.log(v.firstName)
                    })

                    $('.dt_basic').DataTable();
                }

                if(uploadedUser.failed.length > 0)
                {
                    var tbody = $("#failed");
                    $.each(uploadedUser.failed, function(d,v){
                        tbody.append(`<tr>
                        <td>${v.firstName}</td>
                        <td>${v.lastName}</td>
                        <td>${v.gender}</td>
                        <td>${v.message}</td></tr>
                        `)
                    });

                    $('.dt_basics').DataTable();
                }
               
               
                //window.location.reload();
                console.log(response.data);
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
    });
    

    var populateDepartment = function(){
        var depts = $('#vertical-department');
        axios.get(`${url}Departments/GetDepartment`)
                    .then(response => {
                        var d = response.data;
                        $.each(d, function(){
                            depts.append($("<option />").val(this.deptId).text(this.deptName))
                        })
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
    };

    populateDepartment();
})