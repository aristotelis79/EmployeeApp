$(document).ready(function () {
    //Index data table init
    const table = $('#employees-table').DataTable();
    employeesTable.Init(table);

    //Init employee Actions
    employeesActions.Init(employeesTable);
    
});

const employeesTable = {
    _table: null,
    
    Init: function(table) {
        employeesTable._table = table;
    },
    addRow: function(data) {
        employeesTable._table.add( {
            "EmpName": data["EmpName"],
            "EmpDateOfHire": data["EmpDateOfHire"],
            "EmpSupervisorName": data["EmpSupervisorName"]
        } ).draw();
    },
    removeRow: function(data) {
        employeesTable._table.rows( '[data-id="'+data["EmpId"]+'"]' ).remove().draw();
    }
}

const employeesActions = {
    _count: 0,

    Init: function () {

        $('[employee-action="new"]').on('click', this.new);
        $('#employees-table').on('click','[employee-action="details"]', this.details);
        $('#employees-table').on('click', '[employee-action="create"]', this.create);
        $('#employees-table').on('click', '[employee-action="edit"]', this.edit);
        $('#employees-table').on('click','[employee-action="delete"]', this.delete);
    },

    new: function () {
        $("#employee-actions").load("/employee/create/");
    },

    details: function () {
        employeesActions.ajaxAction('get', '/api/employee/' + $(this).attr('data-id'));
    },

    create: function () {
        let data = $("#employee-form").serializeToJSON({associativeArrays: false});
        employeesActions.ajaxAction('post', '/api/employee/', data);
    },

    edit: function () {
        let data = $("#employee-form").serializeToJSON({associativeArrays: false});
        employeesActions.ajaxAction('put', '/api/employee/' + $(this).attr('data-id'), data);
    },

    delete: function () {
        employeesActions.ajaxAction('delete', '/api/employee/' + $(this).attr('data-id'));
    },

    ajaxAction: function (type, url, data) {
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
                debugger;
            },
            error: function (jqXHR, textStatus, errorThrown) {

                debugger;
            }
        }).done(function(response, textStatus, jqXHR){
            if (jqXHR.status === 200) {
                employeesTable.addRow(response);
            }
            if (jqXHR.status === 205) {
                employeesTable.removeRow(data);
            }
        });
    }
};
