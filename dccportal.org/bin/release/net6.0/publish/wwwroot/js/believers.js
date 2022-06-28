$(function () {
    'use strict';
    $('#dt_basic').DataTable({
        columnDefs: [
            {
                className: 'control',
                orderable: false,
                responsivePriority: 2,
                targets: 0
            },
          
        ],
       language: {
            paginate: {
                previous: '&nbsp;',
                next: '&nbsp;'
            }
        }
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

    $("#edit-believer-form").on('click','.btn-submit',function(e){
        e.preventDefault();
        var model = {
            firstName : $("#firstName").val(),
            lastName : $("#lastName").val(),
            sex : $("#sex option:selected").val(),
            setDateOfBirth: $("#dateOfBirth").val(),
            phoneNumber : $("#phoneNumber").val(),
            homeAddressOne : $("#homeAddressOne").val(),
            homeAddressTwo : $("#homeAddressTwo").val(),
            city : $("#city").val(),
            stateName : $("#stateName").val(),
            country : $("#country").val(),
            maritalStatus : $("#maritalStatus option:selected").val(),
            setWeddingAnniversary: $("#weddingAnniversary").val(),
            facebookName : $("#facebookName").val(),
            instagramHandle : $("#instagramHandle").val(),
            twitterHandle : $("#twitterHandle").val(),
            altPhoneNumber : $("#altPhoneNumber").val(),
            email : $("#email").val(),
            setMemberIdString : $("#memberIdString").val()
           }
           axios.put(`${url}Users/EditUser`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    
                } else {
                    
                    toastr['error'](`👋 ${response.data.message}`, 'Error!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    //$('#signin-btn').html("<b>Submit</b>");
                }
           })
           .catch(function (error) {
            processErrors(error)
           });
    });

    $("#select-dept-form").submit(function(e){
        e.preventDefault();
        var model = {
            believerIdString : $("#believerIdString").val(),
            deptId : $('#vertical-department option:selected').val(),
            groups : $('#vertical-group option:selected').val()
        }
        axios.post(`${url}Members/Create`, model)
           .then(response => {
              
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    window.location.reload();
                } else {
                    
                    toastr['error'](`👋 ${response.data.message}`, 'Error!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    //$('#signin-btn').html("<b>Submit</b>");
                }
           })
           .catch(function (error) {
            processErrors(error)
           });
    });

    $("#departments").on('click',".delete-record",function(e){
        e.preventDefault();
        var member = $(this).attr("data-member");
        var dept = $(this).attr("data-dept");

       
        axios.delete(`${url}Members/Delete?setMemberIdString=${member}`)
        .then(response => {
           
             if (response.data.statusCode === 200) {
                 toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                     closeButton: true,
                     tapToDismiss: true,
                     rtl: false
                 });
                 window.location.reload();
             } else {
                 
                 toastr['error'](`👋 ${response.data.message}`, 'Error!', {
                     closeButton: true,
                     tapToDismiss: true,
                     rtl: false
                 });
                 //$('#signin-btn').html("<b>Submit</b>");
             }
        })
        .catch(function (error) {
         processErrors(error)
        });
    });

    $("#new-believer-form").on('click','.btn-submit',function(e){
        e.preventDefault();
        var model = {
            firstName : $("#firstName").val(),
            lastName : $("#lastName").val(),
            sex : $("#sex option:selected").val(),
            setDateOfBirth: $("#dateOfBirth").val(),
            phoneNumber : $("#phoneNumber").val(),
            homeAddressOne : $("#homeAddressOne").val(),
            homeAddressTwo : $("#homeAddressTwo").val(),
            city : $("#city").val(),
            stateName : $("#stateName").val(),
            country : $("#country").val(),
            maritalStatus : $("#maritalStatus option:selected").val(),
            setWeddingAnniversary: $("#weddingAnniversary").val(),
            facebookName : $("#facebookName").val(),
            instagramHandle : $("#instagramHandle").val(),
            twitterHandle : $("#twitterHandle").val(),
            altPhoneNumber : $("#altPhoneNumber").val(),
            email : $("#email").val(),
            setMemberIdString : $("#memberIdString").val()
           }
           axios.post(`${url}Users/AddUser`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    $(".form-group").find("input[type=text], textarea").val("");
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
