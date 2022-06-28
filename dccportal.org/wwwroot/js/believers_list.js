/**
 * DataTables Basic
 */

 $(function () {
    'use strict';
  
    var dt_basic_table = $('#believers-list');
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
        processing: true,
        serverSide: true,
        ajax: {
            url: url + "Users/Believers",
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
                `<a href="/Users/Believers?_memberId=${full['memberIdString']}" class="dropdown-item">` +
                feather.icons['file-text'].toSvg({ class: 'font-small-4 mr-50' }) +
                'Details</a>' +
                `<a href="javascript:;" data-id="${full['memberId']}" class="dropdown-item delete-record">` +
                feather.icons['trash-2'].toSvg({ class: 'font-small-4 mr-50' }) +
                'Delete</a>' +
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
  