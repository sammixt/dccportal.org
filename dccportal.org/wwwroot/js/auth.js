$(function () {
    'use strict';

    var pageLoginForm = $('.auth-login-form');
  

    // jQuery Validation
    // --------------------------------------------------------------------
    if (pageLoginForm.length) {
        pageLoginForm.validate({
            /*
            * ? To enable validation onkeyup
            onkeyup: function (element) {
              $(element).valid();
            },*/
            /*
            * ? To enable validation on focusout
            onfocusout: function (element) {
              $(element).valid();
            }, */
            rules: {
                'login-email': {
                    required: true
                },
                'login-password': {
                    required: true
                }
            }
        });

        pageLoginForm.on('submit', function (e) {
            var isValid = pageLoginForm.valid();
            var _this = $(this);
            e.preventDefault();
            if (isValid) {
              
                var _email = $('#login-email').val();
                var _password = $('#login-password').val();
                var _dept = $('#vertical-dept option:selected').val();

                $('#signin-btn').html("<span class=\"spinner-border spinner-border-sm\" role=\"status\" aria-hidden=\"true\"></span>\
                                            <span class=\"ml-25 align-middle\">Loading...</span>");

                axios.post(`${url}Home/Login`, { userName: _email, password: _password, deptId: _dept })
                    .then(response => {
                        const addedUser = response.data;
                        if (response.data.statusCode === 200) {
                            toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                                closeButton: true,
                                tapToDismiss: true,
                                rtl: false
                            });
                            window.location.href = "/Home/RedirectToLocal";
                        } else {
                            toastr['error'](`👋 ${response.data.message}`, 'Error!', {
                                closeButton: true,
                                tapToDismiss: true,
                                rtl: false
                            });
                            $('#signin-btn').html("<b>Submit</b>");
                        }
                       
                       // window.location.reload();
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

                        $('#signin-btn').html("<b>Submit</b>");
                    });
            }
        });
    }

    
});