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
            url: url + `Members/Members`,
            type: "POST",
            datatype: "json"
        },
        columns: [
          { data: 'memberId' }, // used for sorting so will hide this column
          { data: "firstName", "name": "First Name", "autoWidth": true },
          { data: "lastName", "name": "Last Name", "autoWidth": true },
          { data: "sex", "name": "Gender", "autoWidth": true },
          { data: 'phoneNumber', "name": "Phone Number", "autoWidth": true },
          { data: 'separtment',"name": "Department", "autoWidth": true  },
          { data: 'unit',"name": "Unit", "autoWidth": true  },
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
              return full['department']
            }
          },
          {
            // Label
            targets: 6,
            responsivePriority: 1,
            render: function (data, type, full, meta) {
              return full['unit']
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
})