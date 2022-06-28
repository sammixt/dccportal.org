/**
 * DataTables Basic
 */

 $(function () {
    'use strict';
  
    var dt_basic_table = $('#members-list');
    // DataTable with buttons
    // --------------------------------------------------------------------
  
    if (dt_basic_table.length) {
        dt_basic_table.DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: url + `Departmental/MemberInDept`,
            type: "POST",
            datatype: "json"
        },
        columns: [
          { data: 'memberId' }, // used for sorting so will hide this column
          { data: "firstName", "name": "First Name", "autoWidth": true },
          { data: "lastName", "name": "Last Name", "autoWidth": true },
          { data: "sex", "name": "Gender", "autoWidth": true },
          { data: 'phoneNumber', "name": "Phone Number", "autoWidth": true },
          { data: 'dateOfBirthString',"name": "Date of Birth", "autoWidth": true  },
          { data: '' }
        ],
        columnDefs: [
          {
            // For Responsive
            className: 'control',
            orderable: false,
            responsivePriority: 2,
            targets: 0
          },
          {
            // For Checkboxes
            targets: 1,
            responsivePriority: 1,
            render: function (data, type, full, meta) {
              return full['firstName'];
            }
          },
          
          {
            // Avatar image/badge, Name and post
            targets: 2,
            responsivePriority: 2,
            render: function (data, type, full, meta) {
              
              return full['lastName'];
            }
          },
          {
            responsivePriority: 1,
            targets: 3,
            render: function (data, type, full, meta) {
              
                return full['sex'];
              }

          },
          {
            responsivePriority: 1,
            targets: 4,
            render: function (data, type, full, meta) {
              
                return full['phoneNumber'];
              }

          },
          {
            // Label
            targets: 5,
            responsivePriority: 1,
            render: function (data, type, full, meta) {
              return full['dateOfBirthString']
            }
          },
          {
            // Actions
            targets: -1,
            title: 'Actions',
            orderable: false,
            render: function (data, type, full, meta) {
              return (
                '<div class="d-inline-flex">' +
                '<a class="pr-1 dropdown-toggle hide-arrow text-primary" data-toggle="dropdown">' +
                feather.icons['more-vertical'].toSvg({ class: 'font-small-4' }) +
                '</a>' +
                '<div class="dropdown-menu dropdown-menu-right">' +
                `<a href="/Departmental/MemberDetail?_memberId=${full['memberIdString']}" class="dropdown-item">` +
                feather.icons['file-text'].toSvg({ class: 'font-small-4 mr-50' }) +
                'Details</a>' +
                `<a href="javascript:;" data-id="${full['getMemberInDeptIdString']}" class="dropdown-item delete-record">` +
                feather.icons['trash-2'].toSvg({ class: 'font-small-4 mr-50' }) +
                'Remove Member</a>' +
                `<a href="javascript:;" data-id="${full['memberIdString']}" data-dept="${full['deptId']}" data-name="${full['firstName']} ${full['lastName']}" class="dropdown-item create-login">` +
                feather.icons['trash-2'].toSvg({ class: 'font-small-4 mr-50' }) +
                'Create Login Credentials</a>' +
                '</div>' +
                '</div>' 
              );
            }
          }
        ],
        order: [[2, 'desc']],
            dom:
                '<"d-flex justify-content-between align-items-center header-actions mx-1 row mt-75"' +
                '<"col-lg-12 col-xl-6" l>' +
                '<"col-lg-12 col-xl-6 pl-xl-75 pl-0"<"dt-action-buttons text-xl-right text-lg-left text-md-right text-left d-flex align-items-center justify-content-lg-end align-items-center flex-sm-nowrap flex-wrap mr-1"<"mr-1"f>B>>' +
                '>t' +
                '<"d-flex justify-content-between mx-2 row mb-1"' +
                '<"col-sm-12 col-md-6"i>' +
                '<"col-sm-12 col-md-6"p>' +
                '>',
            language: {
                sLengthMenu: 'Show _MENU_',
                search: 'Search',
                searchPlaceholder: 'Search..'
            },
            
            responsive: {
                details: {
                    display: $.fn.dataTable.Responsive.display.modal({
                        header: function (row) {
                            var data = row.data();
                            return 'Details of ' + data['full_name'];
                        }
                    }),
                    type: 'column',
                    renderer: $.fn.dataTable.Responsive.renderer.tableAll({
                        tableClass: 'table',
                        columnDefs: [
                            {
                                targets: 2,
                                visible: false
                            },
                            {
                                targets: 3,
                                visible: false
                            }
                        ]
                    })
                }
            },
            language: {
                paginate: {
                    
                    previous: '&nbsp;',
                    next: '&nbsp;'
                }
            }
        })
    }
  
 
    
    $("#members-list tbody").on('click','.create-login', function(e){
        e.preventDefault();
        var memberId = $(this).attr("data-id");
        var name = $(this).attr("data-name");
        var dept = $(this).attr("data-dept");
        $("#full_name").text(name);
        $("#setMemberIdString").val(memberId);
        $("#setDeptIdString").val(dept);
        populateRoles();
        $("#modal-slide-in").modal({
            show: true,
            backdrop: 'static',
            keyboard: false
        });
    });

    var populateRoles = function(){
        var depts = $('#vertical-role');
        axios.get(`${url}Members/GetRoles`)
                    .then(response => {
                        var d = response.data;
                        $.each(d, function(){
                            depts.append($("<option />").val(this.roleId).text(this.roleName))
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

    $("#create-login-form").on('click','.data-submit', function(e){
        e.preventDefault();
        var userName = $("#username").val();
        var passw = $("#password").val();
        var confirmPass = $("#confirm-password").val();
        var member = $("#setMemberIdString").val();
        var dept = $("#setDeptIdString").val();
        var role = $("#vertical-role option:selected").val();
        if(passw !== confirmPass){
            callSwal('warning','Password mismatch','Password and Confirm password mismatches');
        }else{
          var  model = {
                userName : userName,
                Password : passw,
                confirmPassword: confirmPass,
                setBelieverIdString : member,
                deptId : dept,
                roleId:role
            }

            axios.post(`${url}Home/CreateLogin`, model)
            .then(response => {
                 if (response.data.statusCode === 200) {
                     toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                         closeButton: true,
                         tapToDismiss: true,
                         rtl: false
                     });
                     $("#create-login-form").find("input[type=text], textarea").val("");
                     $("#modal-slide-in").modal('hide')
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
        }
    });

    //delete record
    $("#members-list tbody").on('click', 'a.delete-record', function (e) {
        e.preventDefault();
        
        var current_item = $(this);
        var id = current_item.attr('data-id');
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
                axios.delete(`${url}Members/Delete?setMemberIdString=${id}`)
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
                    text: 'Believer is save 😀',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-success'
                    }
                });
            }
        });
    
    });
    
    
  });
  