﻿$(document).ready(function () {

    //Index data table init
    const table = $('#employees-table').DataTable();
    employeesTable.Init(table);

    //Init employee Actions
    employeesActions.Init(employeesTable);
    attributeActions.Init();
});

const Helpers = {
    getGuid: function(str) {
        const regex = new RegExp("(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}");
        var match = str.match(regex);;
        return match[0];
    }
}

const employeesTable = {
    
    _table: null,
    
    Init: function(table) {
        employeesTable._table = table;
    },
    updateRow: function(data) {

        const id = data['empId'];

        employeesTable.removeRow(id);

        employeesTable._table.row.add([
            data["empName"],
            data["empDateOfHire"].split("T")[0],
            data["empSupervisorName"],
            `<div class="col-3"><button class="btn btn-sm btn-info" employee-action="details" data-id="${id}">Details</button></div>
            <div class="col-3"><button class="btn btn-sm btn-warning" employee-action="edit" data-id="${id}">Edit</button></div>
            <div class="col-3"><button class="btn btn-sm btn-danger" employee-action="delete" data-id="${id}">Delete</button></div>`
        ]).draw();

        $(`[data-id="${id}"]`).parents('td').addClass('row');
    },
    removeRow: function(id) {
        employeesTable._table.rows($(`[data-id="${id}"]`).parents('tr')).remove().draw();
    }
}

const employeesActions = {

    Init: function () {

        $('[employee-action="new"]').on('click', this.new);
        $('#employees-table').on('click','[employee-action="details"]', this.details);
        $('#employees-table').on('click', '[employee-action="edit"]', this.edit);
        $('#employees-table').on('click','[employee-action="delete"]', this.delete);
        $('#employee-actions').on('click', '[employee-action="create"]', this.create);
        $('#employee-actions').on('click', '[employee-action="update"]', this.update);
    },

    new: function () {
        employeesActions.load("/employee/create/");
    },

    details: function () {
        employeesActions.load(`/employee/details/${$(this).attr('data-id')}`);
    },

    edit: function () {
        employeesActions.load(`/employee/edit/${$(this).attr('data-id')}`);
    },

    create: function () {
        let data = $("#employee-form").serializeToJSON({associativeArrays: false});
        employeesActions.ajax('post', '/api/employees/', data);
    },

    update: function () {
        let data = $("#employee-form").serializeToJSON({associativeArrays: false});
        employeesActions.ajax('put', '/api/employees/' + data["EmpId"], data);
    },

    delete: function () {
        employeesActions.ajax('delete', `/api/employees/${$(this).attr('data-id')}`);
    },

    load: function(url) {
        $("#employee-actions").load(url);
    },

    ajax: function (type, url, data) {
        $.ajax({
            cache: false,
            dataType: 'json',
            "headers": {
                "Content-Type": "application/json"
            },     
            type: type,
            data: JSON.stringify(data),
            url: url,
            success: function (response, textStatus, jqXHR) {
                if (jqXHR.status === 201) {
                    employeesTable.updateRow(response);
                }
                alerts.success();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status === 205) {
                    employeesTable.removeRow(Helpers.getGuid(jqXHR.responseText));
                    $("#employee-actions").html('');
                    alerts.success();
                } else {
                    alerts.error(jqXHR.responseJSON);
                }
            }
        }).done(function(response, textStatus, jqXHR) {
            $("#employee-actions").html('');
        });
    }
};

const attributeActions = {
    
    Init: function () {
        $('#employee-actions').on('click', '[attribute-action="delete"]', this.delete);
        $('#employee-actions').on('click', '[attribute-action="new"]', this.new);
    },
    new: function() {
        let model = $("#employee-form").serializeToJSON({associativeArrays: false});
        attributeActions.load('/employee/addAttribute',model);
    },
    delete: function() {
        var model = $("#employee-form").serializeToJSON({associativeArrays: false});
        var number = $(this).attr('data-attribute-number');
        var data = {
             model: model,
             attributeNumber: number
        };
        attributeActions.load('/employee/removeAttribute',data);
    },
    load: function(url,data) {
        $("#employee-attributes-form").load(url,data);
    }
}

const alerts = {

    getType: function(response, msg) {

        if (response.title && response.title.includes('validation')) {
            
            msg.type = 'warning';

            if (response.errors) {
            
                msg.text = '';
                
                for (var e in response.errors) {
                    var errors = response.errors[e].values();
                    for (const value of errors) {
                        msg.text = msg.text.concat(value,"\n");
                    }
                }
            }
        }
        return msg;
    },
    error: function(response) {

        var msg = {
            type: 'error',
            title: 'Oops...',
            text: 'Something went wrong!'
        };

        if (response) {
            msg = alerts.getType(response,msg);
        } 

        Swal.fire({
            title: msg.title,
            icon: msg.type,
            text: msg.text
        });
    },
    success: function() {
        Swal.fire({
            title:"Success",
            icon: 'success'
        });
    }
}