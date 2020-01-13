$(document).ready(function () {

    //Index data table init
    const table = $('#employees-table').DataTable({
     
        columnDefs:[{targets:1, render:function(data){
            return Helpers.formatDate(data);
        }}]
    });
    employeesTable.Init(table);

    //Init employee Actions
    employeesActions.Init(employeesTable);
});

const Helpers = {
    formatDate: function(dateTimeStr) {
        const datetime = new Date(dateTimeStr);
        return datetime.getFullYear() + "-" + (datetime.getMonth() + 1) + "-" + datetime.getDate();
    },
    getGuid: function(str) {
        const regex = new RegExp('(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}');
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
            data["empDateOfHire"],
            data["empSupervisorName"],
            `<button class="btn btn-info" employee-action="details" data-id="${id}">Details</button>
                    <button class="btn btn-warning" employee-action="edit" data-id="${id}">Edit</button>
                    <button class="btn btn-danger" employee-action="delete" data-id="${id}">Delete</button>`
        ] ).draw();
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
        attributeActions.Init(1);//TODO
    },

    details: function () {
        employeesActions.load("/employee/details/"+ $(this).attr('data-id'));
    },

    edit: function () {
        employeesActions.load("/employee/edit/"+ $(this).attr('data-id'));
        attributeActions.Init($(this).attr('[attributes-number]'));
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
        employeesActions.ajax('delete', '/api/employees/' + $(this).attr('data-id'));
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
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status === 205) {
                    employeesTable.removeRow(Helpers.getGuid(jqXHR.responseText));
                }
            }
        }).done(function(response, textStatus, jqXHR){});
    }
};

const attributeActions = {
    Init: function(count) {
        this.count = count;
        $("#employee-actions").on("click","#add-attribute", attributeActions.addAttribute);
        $("#employee-actions").on('click','[attribute-action="delete"]', attributeActions.deleteAttribute);

        attributeActions.disablePreviousDeleteButtons();
    },
    count: 0,
    addAttribute: function() {
        const $template = $("#attribute-template").clone();

        const $attributeItemHtml = $template.html().replace(/\{0}/g, attributeActions.count);

        $("#additional-attribute").append($attributeItemHtml);

        attributeActions.count += 1;

        attributeActions.disablePreviousDeleteButtons();
    },
    deleteAttribute: function() {
        var attribute = $(this).data("attributeid");
        $("#" + attribute).remove();

        attributeActions.count -= 1;

        attributeActions.disablePreviousDeleteButtons();
    },
    disablePreviousDeleteButtons: function() {
        $('[attribute-action="delete"]').each(function() {
            if ($(this).data("number") < attributeActions.count - 1) {
                $(this).attr("disabled", true);
            } else {
                $(this).attr("disabled", false);
            }
        });
    }
};