$(function () {
    'use strict';
   
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
