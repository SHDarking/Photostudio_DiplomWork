// ajax methods for drawing calendar
document.addEventListener('DOMContentLoaded', function () {
    ajaxQuery(2, setEventsForCalendar);
    var hallSelect = document.querySelector("#HallSelect");
    if (hallSelect != null) {
        hallSelect.addEventListener('change', function () {
            ajaxQuery(2, setEventsForCalendar);
        });
    }
});
function setEventsForCalendar() {
    var currentWeekButton = document.querySelector(".currentWeek");
    if (currentWeekButton != null) {
        currentWeekButton.addEventListener('click', function () {
            ajaxQuery(2, setEventsForCalendar);
        });
    }
    var nextWeekButton = document.querySelector(".nextWeek");
    if (nextWeekButton != null) {
        nextWeekButton.addEventListener('click', function () {
            ajaxQuery(3, setEventsForCalendar, nextWeekButton);
        });
    }
    var prevWeekButton = document.querySelector(".prevWeek");
    if (prevWeekButton != null) {
        prevWeekButton.addEventListener('click', function () {
            if (!this.classList.contains("disabled")) {
                ajaxQuery(1, setEventsForCalendar, prevWeekButton);
            }
        });
    }
}
function ajaxQuery(action, callback, button) {
    if (callback === void 0) { callback = null; }
    if (button === void 0) { button = null; }
    var id = document.getElementById("HallSelect").value;
    var url;
    if (action === 2) {
        url = "/Reserving/GetFreedomHallTime?id=" + id + "&action=" + action;
    }
    else {
        var date = button.parentNode.querySelector("span").id;
        url = "/Reserving/GetFreedomHallTime?id=" + id + "&action=" + action + "&date=" + date;
    }
    var xhr = new XMLHttpRequest();
    xhr.open("GET", url);
    xhr.onload = function () {
        var calendar = document.querySelector("#calendar");
        if (calendar != null) {
            var response = xhr.responseText;
            calendar.childNodes.forEach(function (element) { return element.remove(); });
            calendar.insertAdjacentHTML("afterbegin", response);
            if (callback !== null) {
                callback();
            }
        }
    };
    xhr.send();
}
;
// total cost calculation on web-page
var selectEquipments = document.querySelectorAll(".SelectEquipments input[type=checkbox]");
var selectServices = document.querySelectorAll("#SelectServices input[type=checkbox]");
var selectHallOrDateTime = document.querySelectorAll("input[type=date],.time-select,#HallSelect");
selectEquipments.forEach(function (element) { return element.addEventListener('change', calculateEquipmentCost); });
selectEquipments.forEach(function (element) { return onLoadCounterEvents(element); });
selectServices.forEach(function (element) { return element.addEventListener('change', calculateServicesCost); });
selectHallOrDateTime.forEach(function (element) { return element.addEventListener('change', updateTotalCost); });
function onLoadCounterEvents(element) {
    if (element.checked) {
        element.parentNode.parentNode.querySelector(".counterMinus")
            .addEventListener('click', downgradeCounterValue);
        element.parentNode.parentNode.querySelector(".counterPlus")
            .addEventListener('click', upgradeCounterValue);
    }
}
;
function downgradeCounterValue() {
    var durationRent = getDurationRent();
    var cost = this.parentNode.parentNode.querySelector(".form-check .visually-hidden").value;
    var infoTotalCost = document.getElementById("totalCostValue");
    var totalCostValue = document.querySelector("input[name='Form.TotalCost']");
    var checkbox = this.parentNode.parentNode.querySelector("input[type='checkbox']");
    if (!checkbox.hasAttribute("disabled") && checkbox.checked) {
        var equipmentValue = this.parentNode.querySelector("input");
        var equipmentValueInfo = this.parentNode.querySelector("span");
        var value = Number(equipmentValue.value);
        if (value > 1) {
            var newValue = value - 1;
            equipmentValue.value = newValue.toString();
            equipmentValueInfo.textContent = newValue.toString();
            var totalCost = Number(totalCostValue.value) - (Number(cost) * durationRent);
            infoTotalCost.textContent = totalCost.toString();
            totalCostValue.value = totalCost.toString();
            ;
        }
    }
}
;
function upgradeCounterValue() {
    var durationRent = getDurationRent();
    var cost = this.parentNode.parentNode.querySelector(".form-check .visually-hidden").value;
    var infoTotalCost = document.getElementById("totalCostValue");
    var totalCostValue = document.querySelector("input[name='Form.TotalCost']");
    var checkbox = this.parentNode.parentNode.querySelector("input[type='checkbox']");
    if (!checkbox.hasAttribute("disabled") && checkbox.checked) {
        var equipmentValue = this.parentNode.querySelector("input");
        var equipmentValueInfo = this.parentNode.querySelector("span");
        var value = Number(equipmentValue.value);
        if (value < 500) {
            var newValue = value + 1;
            equipmentValue.value = newValue.toString();
            equipmentValueInfo.textContent = newValue.toString();
            var totalCost = (Number(cost) * durationRent) + Number(totalCostValue.value);
            infoTotalCost.textContent = totalCost.toString();
            totalCostValue.value = totalCost.toString();
        }
    }
}
;
function calculateEquipmentCost() {
    var rentDuration = getDurationRent();
    var cost = this.parentNode.querySelector("input.visually-hidden").value;
    var infoTotalCost = document.getElementById("totalCostValue");
    var totalCostValue = document.querySelector("input[name='Form.TotalCost']");
    var equipmentValue = this.parentNode.parentNode.querySelector("input[name='Form.CountEquipments']");
    var equipmentValueInfo = this.parentNode.parentNode.querySelector("span.counterInfo");
    var value = Number(equipmentValue.value);
    var totalCost = 0;
    if (this.checked) {
        this.parentNode.parentNode.querySelector(".counterMinus")
            .addEventListener('click', downgradeCounterValue);
        this.parentNode.parentNode.querySelector(".counterPlus")
            .addEventListener('click', upgradeCounterValue);
        var newValue = 1;
        equipmentValue.value = newValue.toString();
        equipmentValueInfo.textContent = newValue.toString();
        totalCost = (Number(cost) * rentDuration * newValue) + Number(totalCostValue.value);
        infoTotalCost.textContent = totalCost.toString();
        totalCostValue.value = totalCost.toString();
    }
    else {
        this.parentNode.parentNode.querySelector(".counterMinus")
            .removeEventListener('click', downgradeCounterValue);
        this.parentNode.parentNode.querySelector(".counterPlus")
            .removeEventListener('click', upgradeCounterValue);
        totalCost = Number(totalCostValue.value) - (Number(cost) * rentDuration * value);
        infoTotalCost.textContent = totalCost.toString();
        totalCostValue.value = totalCost.toString();
        var newValue = 0;
        equipmentValue.value = newValue.toString();
        equipmentValueInfo.textContent = newValue.toString();
    }
}
;
function calculateServicesCost() {
    var selectBox = this.parentNode.querySelector("input.visually-hidden");
    var cost = selectBox.value;
    var infoTotalCost = document.getElementById("totalCostValue");
    var totalCostValue = document.querySelector("input[name='Form.TotalCost']");
    var totalCost = 0;
    if (this.checked) {
        totalCost = Number(cost) + Number(totalCostValue.value);
        infoTotalCost.textContent = totalCost.toString();
        totalCostValue.value = totalCost.toString();
    }
    else {
        totalCost = Number(totalCostValue.value) - Number(cost);
        infoTotalCost.textContent = totalCost.toString();
        totalCostValue.value = totalCost.toString();
    }
}
;
function updateTotalCost() {
    var totalCost = calculateTotalCost();
    document.getElementById("totalCostValue").textContent = totalCost.toString();
    document.querySelector("input[name='Form.TotalCost']").value = totalCost.toString();
}
;
function getDurationRent() {
    var boxStartDateRent = document.getElementById("startDateHallsRent");
    var boxStartTimeRent = document.getElementById("startTimeHallsRent");
    var boxEndDateRent = document.getElementById("endDateHallsRent");
    var boxEndTimeRent = document.getElementById("endTimeHallsRent");
    var startDateTimeRent = new Date(new Date(boxStartDateRent.value).setHours(Number(boxStartTimeRent.value)));
    var endDateTimeRent = new Date(new Date(boxEndDateRent.value).setHours(Number(boxEndTimeRent.value)));
    var rentDuration = (Number(endDateTimeRent) - Number(startDateTimeRent)) / (3600 * 1000);
    return rentDuration;
}
;
function calculateTotalCost() {
    var rentDuration = getDurationRent();
    var hallSelectedBox = document.getElementById("HallSelect");
    var hallPriceBox = document.getElementById("hallInfo");
    var selectHallsPrice = 0;
    var hallPriceOptionBox = hallPriceBox.querySelectorAll("option");
    for (var i = 0; i < hallPriceOptionBox.length; i++) {
        if (Number(hallPriceOptionBox[i].value) === Number(hallSelectedBox.value)) {
            selectHallsPrice = Number(hallPriceOptionBox[i].textContent);
        }
    }
    var totalCost = selectHallsPrice * rentDuration;
    var checkboxCollection = document.querySelectorAll("input[type='checkbox']");
    for (var i = 0; i < checkboxCollection.length; i++) {
        var cost = Number(checkboxCollection[i].parentNode.querySelector("input").value);
        if (checkboxCollection[i].checked && checkboxCollection[i].name === "Form.SelectedServices") {
            totalCost += cost;
        }
        else if (checkboxCollection[i].checked && checkboxCollection[i].name === "Form.SelectedEquipments") {
            var countCurrentEquipment = checkboxCollection[i].parentNode.parentNode.querySelector("input[name='Form.CountEquipments']").value;
            totalCost += cost * rentDuration * Number(countCurrentEquipment);
        }
    }
    return totalCost;
}
;
// adminpanel settings
var manageControl = document.querySelector("input[name='manageControlCheck']");
if (manageControl != null) {
    manageControl.addEventListener('change', function () {
        if (manageControl.checked) {
            selectEquipments.forEach(function (element) { return element.removeAttribute("disabled"); });
            selectServices.forEach(function (element) { return element.removeAttribute("disabled"); });
            selectHallOrDateTime.forEach(function (element) { return element.removeAttribute("disabled"); });
            document.querySelector("input[name='Form.TotalCost']").removeAttribute("disabled");
            document.querySelectorAll("input[name='Form.CountEquipments']").forEach(function (element) { return element.removeAttribute("disabled"); });
        }
        else {
            selectEquipments.forEach(function (element) { return element.setAttribute("disabled", ""); });
            selectServices.forEach(function (element) { return element.setAttribute("disabled", ""); });
            selectHallOrDateTime.forEach(function (element) { return element.setAttribute("disabled", ""); });
            document.querySelector("input[name='Form.TotalCost']").setAttribute("disabled", "");
            document.querySelectorAll("input[name='Form.CountEquipments']").forEach(function (element) { return element.setAttribute("disabled", ""); });
        }
    });
}
//# sourceMappingURL=OrderScript.js.map