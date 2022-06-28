/**
 * DataTables Basic
 */

 $(function () {
    'use strict';
  
    var dt_basic_table = $('#dept_list');
    
    // DataTable with buttons
    // --------------------------------------------------------------------
  
    if (dt_basic_table.length) {
        dt_basic_table.DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: url + "Departments/Departments",
            type: "POST",
            datatype: "json"
        },
        columns: [
          { data: 'deptId' }, // used for sorting so will hide this column
          { data: "deptName", "name": "Department", "autoWidth": true },
          { data: "deptDesc", "name": "Description", "autoWidth": true },
          { data: "shortCode", "name": "ShortCode", "autoWidth": true },
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
              return full['deptName'];
            }
          },
          
          {
            // Avatar image/badge, Name and post
            targets: 2,
            responsivePriority: 2,
            render: function (data, type, full, meta) {
              
              return full['deptDesc'];
            }
          },
          {
            responsivePriority: 1,
            targets: 3,
            render: function (data, type, full, meta) {
              
                return full['shortCode'];
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
                `<a href="javascript:;" data-dept="${full['deptIdString']}" class="dropdown-item details">` +
                feather.icons['file-text'].toSvg({ class: 'font-small-4 mr-50' }) +
                'Details</a>' +
                `<a href="javascript:;" data-dept="${full['deptIdString']}" class="dropdown-item delete-record">` +
                feather.icons['trash-2'].toSvg({ class: 'font-small-4 mr-50' }) +
                'Delete</a>' +
                `<a href="/Members/Department?dept=${full['deptIdString']}" data-dept="${full['deptIdString']}" class="dropdown-item">` +
                feather.icons['users'].toSvg({ class: 'font-small-4 mr-50' }) +
                'Members in Department</a>' +
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
  
    //delete record
    $("#believers-list tbody").on('click', 'a.delete-record', function (e) {
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
                axios.delete(`${url}Users/DeleteBeliever/${id}`)
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
                    text: 'Department is save 😀',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-success'
                    }
                });
            }
        });
    
    });
    

    $("#new-department-form").on('click','.btn-submit',function(e){
        e.preventDefault();
        var model = {
            deptName : $("#vertical-dept-name").val(),
            deptDesc : $("#vertical-dept-desc").val(),
            vision: $("#vertical-dept-vision").val(),
            shortCode : $("#vertical-dept-shortcode").val()
           }
           axios.post(`${url}Departments/AddDepartment`, model)
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

    $("#dept_list tbody").on('click','.details', function(e){
        e.preventDefault();
        var dept = $(this).attr("data-dept");
        
        axios.get(`${url}Departments/GetDepartmentDetail?SetDeptIdString=${dept}`)
        .then(response => {
            var d = response.data;
            $("#dept-name").val(d.deptName),
            $("#dept-desc").val(d.deptDesc),
            $("#dept-vision").val(d.vision),
            $("#dept-shortcode").val(d.shortCode)
            $("#setDeptIdString").val(d.deptIdString)

            $("#modal-slide-in").modal({
                show: true,
                backdrop: 'static',
                keyboard: false
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
    });

    $("#edit-user-form").on('click','.data-submit',function(e){
        e.preventDefault();
        var model = {
            deptName : $("#dept-name").val(),
            deptDesc : $("#dept-desc").val(),
            vision: $("#dept-vision").val(),
            shortCode : $("#dept-shortcode").val(),
            SetDeptIdString: $("#setDeptIdString").val()
           }
           axios.put(`${url}Departments/EditDept`, model)
           .then(response => {
                if (response.data.statusCode === 200) {
                    toastr['success'](`👋 ${response.data.message}`, 'Success!', {
                        closeButton: true,
                        tapToDismiss: true,
                        rtl: false
                    });
                    $('#modal-slide-in').modal('hide')
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

    //delete record
    $("#dept_list tbody").on('click', 'a.delete-record', function (e) {
        e.preventDefault();
        
        var dept = $(this).attr("data-dept");
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
                axios.delete(`${url}Departments/DeleteDepartment?SetDeptIdString=${dept}`)
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
                    text: 'Department is save 😀',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-success'
                    }
                });
            }
        });
    
    });
    
    
  });
  