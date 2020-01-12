$(document).ready(function () {
    //Index data table init
    const employeesTable = $('#employees-table').DataTable();

    //Init employee Actions
    employeesActions.Init(employeesTable);
});

const employeesActions = {
    _count: 0,
    _table: null,

    Init: function (table) {
        this._table = table;
        $('[employee-action="details"]').on('click', this.details);
        $('[employee-action="new"]').on('click', this.new);
        $('#employee-actions').on('click', '[employee-action="create"]', this.create);
        $('#employee-actions').on('click', '[employee-action="edit"]', this.edit);
        $('[employee-action="delete"]').on('click', this.delete);
    },
    new: function () {
        $("#employee-actions").load("employee/create");
    },
    details: function () {
        employeesActions.ajaxAction('get', 'api/employee/' + $(this).attr('data-id'));
    },
    create: function () {
        var form = $('#employee-form')[0].elements;
        var data = ToJsonObj.formToJSON(form);
        employeesActions.ajaxAction('post', 'api/employee/', data);
    },
    edit: function () {
        employeesActions.ajaxAction('put', 'api/employee/' + $(this).attr('data-id'));
    },
    delete: function () {
        employeesActions.ajaxAction('delete', 'api/employee/' + $(this).attr('data-id'));
    },
    ajaxAction: function (type, url, data) {
        $.ajax({
            cache: false,
            dataType: 'json',
            type: type,
            data: data,
            url: url,
            success: function (data, textStatus, jqXHR) {
                debugger;
                _table.ajax.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                debugger;
            }
        });
    }
};
