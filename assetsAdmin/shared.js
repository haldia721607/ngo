var hpls = (function (scope) {
    scope.url = `/home`;

    scope.ajaxCall = function (method, url, data, dataType, f, headers = null, asyncHit = true, showWaiting = true) {
        if (showWaiting) {
            tpc.waitToggle();
        }
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
    scope.login = function () {
        $("#responsemsg", "#loginform").html("");
        var url = "/home/authenticate";
        var data = $("#loginform").serialize();
        hplc.ajaxCall("POST", url, data, "text", function (d) {
            var jsonResponse = JSON.parse(d);
            if (jsonResponse == "Fail") {
                $("#loginForm").val("");
                $("#msg").text("username and password is wrong..!");
            }
            else {
                window.location.href = "/admin/dashboard";
                $("#msg").hide();
            }
        });
    };

    scope.UserProfile = function () {
        var user = [];
        var formSubmitHandler = $.noop;
        $(".btn-add-new").on("click", function () {
            $("#StateId").prop("disabled", true);
            $("#CityId").prop("disabled", true);
            hplc.ajaxCall("GET", scope.url + `/userprofile`, {}, "text", function (d) {
                var result = JSON.parse(d);
                user = result.User;
                if (result.Status !== undefined) {
                    //Show error
                } else {
                    //Bind Countries
                    $.each(result.Countries, function (index, value) {
                        $("#CountryId").append(`<option value="${value.CountryId}" >${value.CountryName}</option>`)
                    });
                }
            });
            formSubmitHandler = function () {
                manageUserMaster(user);
            };
            $("#UserProfileform").trigger('reset');
            $("#UserProfileform").validate().resetForm();
            $("#userprofilemsg").text("");
            $('#modelForm').modal('show');
        });

        var manageUserMaster = function (user) {
            $.each($(`input, textarea, select`, "#modelForm form"), function (i, o) {
                if (o.name === "CountryId") { user[o.name] = $("#CountryId").val(); }
                else if (o.name === "StateId") { user[o.name] = $("#StateId").val(); }
                else if (o.name === "CityId") { user[o.name] = $("#CityId").val(); }
                else { user[o.name] = o.value; }
            });

            $("#responsemsg", "#UserProfileform").html("");
            hplc.ajaxCall("POST", `${scope.url}/userprofile`, user, "text", function (d) {
                var jsonResponse = JSON.parse(d);
                if (jsonResponse == "sucess") {
                    $("#userprofilemsg").text("User Registered Sucessfully..!");
                }
                else if (jsonResponse == "exist") {
                    $("#userprofilemsg").text("Mobile Number Already Exists..!");
                }
                else {
                    $("#UserProfileform").val("");
                    $("#userprofilemsg").text("Technical error please try after some time..!");
                }
            });
        };

        $("#UserProfileform").submit(function (event) {
            event.preventDefault();
        }).validate({
            rules: {
                FirstName: { required: true },
                MiddleName: { required: true },
                LastName: { required: true },
                DOB: { required: true },
                MobileNo: { required: true },
                EmailId: { required: true },
                CountryId: { required: true, checkddlValue: '0' },
                StateId: { required: true, checkddlValue: '0' },
                CityId: { required: true, checkddlValue: '0' },
                PinCode: { required: true },
                Address: { required: true },
                AadharNumber: { required: true },
            },
            messages: {
                FirstName: "Please enter first name",
                MiddleName: "Please enter middle name",
                LastName: "Please enter last name",
                DOB: "Please select date.",
                MobileNo: "Please enter mobile number",
                EmailId: "Please enter email id",
                CountryId: { required: '', checkddlValue: 'Please select country.' },
                StateId: { required: '', checkddlValue: 'Please select state.' },
                CityId: { required: '', checkddlValue: 'Please select city.' },
                PinCode: "Please enter pin code.",
                Address: "Please enter address.",
                AadharNumber: "Please enter aadhar number."
            },
            submitHandler: function () {
                formSubmitHandler();
            }
        });
        $.validator.addMethod("checkddlValue", function (value, element) {
            if (element.name === "CountryId") { if (value !== "0") { return true } else { return false } }
            if (element.name === "StateId") { if (value !== "0") { return true } else { return false } }
            if (element.name === "CityId") { if (value !== "0") { return true } else { return false } }
        }, "");

        $("#CountryId").change(function () {
            $("#StateId").prop("disabled", true);
            $("#CityId").prop("disabled", true);
            var countryId = $('option:selected', this).val();
            if (countryId > 0) {
                hplc.ajaxCall("GET", scope.url + `/getstate?countryId=${$('option:selected', this).val()}`, {}, "text", function (d) {
                    var result = JSON.parse(d);
                    if (result.Status !== undefined) {
                        //Show error
                    } else {
                        var state = $('#StateId');
                        state.html("");
                        state.append($(`<option value="0" selected>Select State</option>`));
                        //Bind States
                        $.each(result, function (index, value) {
                            state.append(`<option value="${value.StateId}" >${value.StateName}</option>`)
                        });
                        $("#StateId").prop("disabled", false);
                        //City
                        var city = $('#CityId');
                        city.html("");
                        city.append($(`<option value="0" selected>Select City</option>`));
                        $("#CityId").prop("disabled", true);
                    }
                });
            } else {
                //Disabled Stae
                var state = $('#StateId');
                state.html("");
                state.append($(`<option value="0" selected>Select State</option>`));
                $("#StateId").prop("disabled", true);
                //Disabled City
                var city = $('#CityId');
                city.html("");
                city.append($(`<option value="0" selected>Select City</option>`));
                $("#CityId").prop("disabled", true);
            }
        });
        $("#StateId").change(function () {
            var stateId = $('option:selected', this).val();
            if (stateId > 0) {
                hplc.ajaxCall("GET", scope.url + `/getcity?stateId=${$('option:selected', this).val()}`, {}, "text", function (d) {
                    var result = JSON.parse(d);
                    if (result.Status !== undefined) {
                        //Show error
                    } else {
                        var city = $('#CityId');
                        city.html("");
                        city.append($(`<option value="0" selected>Select City</option>`));
                        //Bind States
                        $.each(result, function (index, value) {
                            city.append(`<option value="${value.CityId}" >${value.CityName}</option>`)
                        });
                        $("#CityId").prop("disabled", false);
                    }
                });
            } else {
                var city = $('#CityId');
                city.html("");
                city.append($(`<option value="0" selected>Select City</option>`));
                $("#CityId").prop("disabled", true);
            }
        });
    };
    return scope;

})(hpls || {});

$(function () {
    $("#loginform").validate({
        submitHandler: function () { hpls.login() },
        rules: {
            loginid: { required: true },
            password: { required: true },
        },
        messages: {
            loginid: "please enter loginid",
            password: "please enter password"
        }
    });
});

