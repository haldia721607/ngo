var hplc = (function (scope) {
    scope.ajaxCall = function (method, url, data, dataType, f, headers = null, asyncHit = true) {
        $.ajax({
            type: method,
            url: url,
            data: data,
            headers: headers,
            dataType: dataType,
            async: asyncHit,
            success: function (d) {
                f(d);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                f("");
            }
        });
    };
    scope.grid_options = {
        width: "100%",
        height: "auto",
        filtering: true,
        editing: false,
        sorting: true,
        autoload: true,
        pageSize: 15,
        pageButtonCount: 5,
        paging: true
    };
    scope.filteOnKeyPress = function (grid) {
        $(":input", $(grid)).keydown(function () {
            var self = this;
            if (self.timeout)
                clearTimeout(self.timeout);

            if (self.value.length === 0)
                $(grid).jsGrid('loadData');

            self.timeout = setTimeout(function () {
                $(grid).jsGrid('loadData');
            }, 200);
        });
        $("tr:has(td i.inactive)", $(grid)).addClass("inactive");
    };
    scope.validateValue = function (obj, sFormat) {
        if (!($(obj).val() === undefined)) {

            var nameValidation = /^[a-zA-Z]*$/g;
            var numberValidation = /^\d+$/;

            if (scope.isEmpty($(obj).val()) && sFormat !== "RDOLST")
                return false;
            else if (sFormat === "NAME" && ($(obj).val().length < 2 || $(obj).val().length > 25))
                return false;
            else if (sFormat === "EMAIL" && !scope.validateEmail($(obj).val()))
                return false;
            else if (sFormat === "PHONE" && ($(obj).val().length < 9 || $(obj).val().length > 15 || !numberValidation.test($(obj).val())))
                return false;
            else if (sFormat === "DDL" && $(obj).val() === 0)
                return false;
            else if (sFormat === "DDLHH" && $(obj).val() === "HH")
                return false;
            else if (sFormat === "DDLMM" && $(obj).val() === "MM")
                return false;
            else if (sFormat === "RDOLST" && $("#" + $(obj).attr("id") + " input:radio:checked").length === 0)
                return false;
            else if (sFormat === "PWD" && !scope.checkPasswordComplexity($(obj).val()))
                return false;
            else if (sFormat === "PRICE" && !scope.validateDecimal(obj, sMessage))
                return false;
            else if (sFormat === "ALPHA" && !nameValidation.test($(obj).val()))
                return false;
            else if (sFormat === "ADD" && $(obj).val().length < 1 && $(obj).val() !== "" && $(obj).val() !== null)
                return false;
            else if (sFormat === "POSTCODE" && $(obj).val().length < 6 && $(obj).val() !== "" && $(obj).val() !== null)
                return false;
            else if (sFormat === "CITY" && $(obj).val().length < 1 && $(obj).val() !== "" && $(obj).val() !== null)
                return false;
        }

        return true;
    };

    scope.checkValue = function (obj, sMessage, sFormat) {
        if (!($(obj).val() === undefined || scope.validateObject.status === false)) {
            if (scope.isEmpty($(obj).val()) && sFormat !== "RDOLST")
                scope.validateObject.status = false;
            else if (sFormat === "NAME" && ($(obj).val().length < 2 || $(obj).val().length > 25))
                scope.validateObject.status = false;
            else if (sFormat === "EMAIL" && !scope.validateEmail($(obj).val()))
                scope.validateObject.status = false;
            else if (sFormat === "PHONE" && ($(obj).val().length < 10 || $(obj).val().length > 15))
                scope.validateObject.status = false;
            else if (sFormat === "DDL" && $(obj).val() === 0)
                scope.validateObject.status = false;
            else if (sFormat === "DDLHH" && $(obj).val() === "HH")
                scope.validateObject.status = false;
            else if (sFormat === "DDLMM" && $(obj).val() === "MM")
                scope.validateObject.status = false;
            else if (sFormat === "RDOLST" && $("#" + $(obj).attr("id") + " input:radio:checked").length == 0)
                scope.validateObject.status = false;
            else if (sFormat === "PWD" && !scope.checkPasswordComplexity($(obj).val()))
                scope.validateObject.status = false;
            else if (sFormat === "PRICE" && !scope.validateDecimal(obj, sMessage))
                scope.validateObject.status = false;

            if (!scope.validateObject.status) {
                scope.validateObject.objElement = sFormat !== "RDOLST" ? $(obj) : $("#" + $(obj).attr("id") + " input:first");
                if ($(scope.validateObject.objElement).attr("data-tp-msg") !== undefined && $(scope.validateObject.objElement).attr("data-tp-msg") !== "")
                    scope.validateObject.errMessage = $(scope.validateObject.objElement).attr("data-tp-msg");
                else
                    scope.validateObject.errMessage = sMessage;
            }
        }
    };

    scope.validateEmail = function (strEmail) {
        validRegExp = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
        return validRegExp.test(strEmail);
    };

    return scope;
})(hplc || {});

function exportInExcel(elem) {
    var table = document.getElementById("rpt-table");
    var html = table.outerHTML;
    var url = 'data:application/vnd.ms-excel,' + escape(html); // Set your html table into url 
    elem.setAttribute("href", url);
    elem.setAttribute("download", "export.xls"); // Choose the file name
    return false;
}

$(function () {
    $("[tp-type]").keypress(function (e) {
        var bReturn = false;
        var k = document.all ? e.keyCode : e.which;
        var sType = $(this).attr("tp-type");
        if (sType === "alpha")
            bReturn = (k > 64 && k < 91) || (k > 96 && k < 123);
        if (sType === "numeric")
            bReturn = (k > 47 && k < 58);
        else if (sType === "alphamuneric" || sType === "alphanumeric")
            bReturn = ((k > 47 && k < 58) || (k > 64 && k < 91) || (k > 96 && k < 123) || k == 0);
        if (sType === "name")
            bReturn = ((k > 64 && k < 91) || (k > 96 && k < 123) || (k == 32)) && ($(this).val().length < 21);
        else if (sType === "email") {
            var char = String.fromCharCode(k).toLowerCase()
            bReturn = ((k > 47 && k < 58) || (k > 64 && k < 91) || (k > 96 && k < 123) || k === 0 || ("@_-.".indexOf(char) !== -1));
        }

        if (!bReturn && $(this).attr("tp-spl-char") !== undefined) {
            var validchars = $(this).attr("tp-spl-char").toLowerCase();
            var char = String.fromCharCode(k).toLowerCase();
            bReturn = (validchars.indexOf(char) !== -1);
        }

        return bReturn;
    });

    var date_input = $('.clsDate'); //our date input has the name "date"
    var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
    var options = {
        format: 'dd/mm/yyyy',
        container: container,
        todayHighlight: true,
        autoclose: true,
    };
    date_input.datepicker(options);
});