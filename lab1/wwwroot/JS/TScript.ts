// ajax methods for drawing calendar

document.addEventListener('DOMContentLoaded', function () {
    ajaxQuery(2, setEventsForCalendar);

    let hallSelect: HTMLSelectElement = document.querySelector("#HallSelect") as HTMLSelectElement | null;

    if (hallSelect != null) {
        hallSelect.addEventListener('change', function () {
            ajaxQuery(2, setEventsForCalendar);
        });
    }
});

function setEventsForCalendar() {

    let currentWeekButton: HTMLDivElement = document.querySelector(".currentWeek") as HTMLDivElement | null;
    if (currentWeekButton != null) {
        currentWeekButton.addEventListener('click', function () {
            ajaxQuery(2, setEventsForCalendar);
        });
    }
    let nextWeekButton: HTMLDivElement = document.querySelector(".nextWeek") as HTMLDivElement | null;
    if (nextWeekButton != null) {
        nextWeekButton.addEventListener('click', function () {
            ajaxQuery(3, setEventsForCalendar, nextWeekButton);
        });
    }
    let prevWeekButton: HTMLDivElement = document.querySelector(".prevWeek") as HTMLDivElement | null;
    if (prevWeekButton != null) {
        prevWeekButton.addEventListener('click', function () {
            if (!this.classList.contains("disabled")) {
                ajaxQuery(1, setEventsForCalendar, prevWeekButton);
            }
        });
    }
}

function ajaxQuery(action: number, callback: Function = null, button: HTMLDivElement = null) {


    let id = (document.getElementById("HallSelect") as HTMLSelectElement).value;
    let url: string;

    if (action === 2) {
        url = "/Reserving/GetFreedomHallTime?id=" + id + "&action=" + action;
    }
    else {
        let date = (button.parentNode.querySelector("span") as HTMLSpanElement | null).id;
        url = "/Reserving/GetFreedomHallTime?id=" + id + "&action=" + action + "&date=" + date;
    }

    let xhr = new XMLHttpRequest();
    xhr.open("GET", url);

    xhr.onload = () => {

        let calendar = document.querySelector("#calendar") as HTMLDivElement | null;
        if (calendar != null) {
            let response = xhr.responseText;
            calendar.childNodes.forEach(element => element.remove());
            calendar.insertAdjacentHTML("afterbegin", response);
            if (callback !== null) {
                callback();
            }
        }
    };
    xhr.send();
};


// total cost calculation on web-page

const selectEquipemts = document.querySelectorAll(".SelectEquipments input[type=checkbox]");
const selectServices = document.querySelectorAll("#SelectServices input[type=checkbox]");
const selectHallOrDateTime = document.querySelectorAll("input[type=date],.time-select,#HallSelect");

selectEquipemts.forEach(element => element.addEventListener('change', calculateEquipmentCost));
selectServices.forEach(element => element.addEventListener('change', calculateServicesCost));
selectHallOrDateTime.forEach(element => element.addEventListener('change', updateTotalCost));

function downgradeCounterValue() {
    let durationRent = getDurationRent();
    let cost: string = (this.parentNode.parentNode.querySelector(".form-check .visually-hidden") as HTMLInputElement).value;

    let infoTotalCost: HTMLSpanElement = document.getElementById("totalCostValue") as HTMLSpanElement | null;
    let totalCostValue: HTMLInputElement = document.querySelector("input[name='TotalCost']") as HTMLInputElement | null;


    let checkbox: HTMLInputElement = this.parentNode.parentNode.querySelector("input[type='checkbox']") as HTMLInputElement | null;
    if (checkbox != null && checkbox.checked) {
        let equipmentValue: HTMLInputElement = this.parentNode.querySelector("input") as HTMLInputElement | null;
        let equipmentValueInfo: HTMLSpanElement = this.parentNode.querySelector("span") as HTMLSpanElement | null;

        let value = Number(equipmentValue.value);

        if (value > 1) {
            let newValue = value - 1;
            equipmentValue.value = newValue.toString();
            equipmentValueInfo.textContent = newValue.toString();
            let totalCost = Number(totalCostValue.value) - (Number(cost) * durationRent) ;
            infoTotalCost.textContent = totalCost.toString();
            totalCostValue.value = totalCost.toString();
            ;
        }
    }
};

function upgradeCounterValue() {

    let durationRent = getDurationRent();
    let cost: string = (this.parentNode.parentNode.querySelector(".form-check .visually-hidden") as HTMLInputElement).value;

    let infoTotalCost: HTMLSpanElement = document.getElementById("totalCostValue") as HTMLSpanElement | null;
    let totalCostValue: HTMLInputElement = document.querySelector("input[name='TotalCost']") as HTMLInputElement | null;


    let checkbox: HTMLInputElement = this.parentNode.parentNode.querySelector("input[type='checkbox']") as HTMLInputElement | null;

    if (checkbox != null && checkbox.checked) {
        let equipmentValue: HTMLInputElement = this.parentNode.querySelector("input") as HTMLInputElement | null;
        let equipmentValueInfo: HTMLSpanElement = this.parentNode.querySelector("span") as HTMLSpanElement | null;

        let value = Number(equipmentValue.value);

        if (value < 500) {
            let newValue = value + 1;
            equipmentValue.value = newValue.toString();
            equipmentValueInfo.textContent = newValue.toString();
            let totalCost = (Number(cost) * durationRent) + Number(totalCostValue.value) ;
            infoTotalCost.textContent = totalCost.toString();
            totalCostValue.value = totalCost.toString();
            
        }
    }
    
    
};

function calculateEquipmentCost() {
    let rentDuration = getDurationRent();
    
    let cost: string = (this.parentNode.querySelector("input.visually-hidden") as HTMLInputElement | null).value;

    let infoTotalCost: HTMLSpanElement = document.getElementById("totalCostValue") as HTMLSpanElement | null;
    let totalCostValue: HTMLInputElement = document.querySelector("input[name='TotalCost']") as HTMLInputElement | null;

    

    let equipmentValue: HTMLInputElement = this.parentNode.parentNode.querySelector("input[name='CountEquipments']") as HTMLInputElement | null;
    let equipmentValueInfo: HTMLSpanElement = this.parentNode.parentNode.querySelector("span.counterInfo") as HTMLSpanElement | null;

    let value = Number(equipmentValue.value);
    let totalCost = 0;

    if (this.checked) {
        this.parentNode.parentNode.querySelector(".counterMinus")
            .addEventListener('click', downgradeCounterValue);
        this.parentNode.parentNode.querySelector(".counterPlus")
            .addEventListener('click', upgradeCounterValue);
        let newValue = 1;
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

        let newValue = 0;
        equipmentValue.value = newValue.toString();
        equipmentValueInfo.textContent = newValue.toString();
    }
    
};

function calculateServicesCost() {
    let selectBox = this.parentNode.querySelector("input.visually-hidden");
    let cost = selectBox.value;
    let infoTotalCost: HTMLSpanElement = document.getElementById("totalCostValue") as HTMLSpanElement | null;
    let totalCostValue = document.querySelector("input[name='TotalCost']") as HTMLInputElement | null;
    let totalCost = 0;

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
};

function updateTotalCost() {
    let totalCost = calculateTotalCost();
    document.getElementById("totalCostValue").textContent = totalCost.toString();
    (document.querySelector("input[name='TotalCost']") as HTMLInputElement | null).value  = totalCost.toString();
};

function getDurationRent() {
    let boxStartDateRent: HTMLInputElement = document.getElementById("startDateHallsRent") as HTMLInputElement | null;
    let boxStartTimeRent: HTMLSelectElement = document.getElementById("startTimeHallsRent") as HTMLSelectElement | null;

    let boxEndDateRent: HTMLInputElement = document.getElementById("endDateHallsRent") as HTMLInputElement | null;
    let boxEndTimeRent: HTMLSelectElement = document.getElementById("endTimeHallsRent") as HTMLSelectElement | null;

    let startDateTimeRent: Date = new Date(new Date(boxStartDateRent.value).setHours(Number(boxStartTimeRent.value)));
    let endDateTimeRent: Date = new Date(new Date(boxEndDateRent.value).setHours(Number(boxEndTimeRent.value)));
    let rentDuration = (Number(endDateTimeRent) - Number(startDateTimeRent)) / (3600 * 1000);
    return rentDuration;
};

function calculateTotalCost() {
    let rentDuration = getDurationRent();

    let hallSelectedBox: HTMLSelectElement = document.getElementById("HallSelect") as HTMLSelectElement | null;
    let hallPriceBox: HTMLSelectElement = document.getElementById("hallInfo") as HTMLSelectElement | null;
    let selectHallsPrice = 0;

    let hallPriceOptionBox = hallPriceBox.querySelectorAll("option");

    for (let i = 0; i < hallPriceOptionBox.length; i++) {
        if (Number(hallPriceOptionBox[i].value) === Number(hallSelectedBox.value)) {
            selectHallsPrice = Number(hallPriceOptionBox[i].textContent);
        } 
    }
    
    let totalCost = selectHallsPrice * rentDuration;

    let checkboxCollection: NodeListOf<HTMLInputElement> = document.querySelectorAll("input[type='checkbox']") as NodeListOf<HTMLInputElement> | null;

    for (let i = 0; i < checkboxCollection.length; i++) {
        let cost = Number(checkboxCollection[i].parentNode.querySelector("input").value);
        if (checkboxCollection[i].checked && checkboxCollection[i].name === "SelectedServices") {
            totalCost += cost;
        }
        else if (checkboxCollection[i].checked && checkboxCollection[i].name === "SelectedEquipments") {
            let countCurrentEquipment = (checkboxCollection[i].parentNode.parentNode.querySelector("input[name='CountEquipments']") as HTMLInputElement | null).value;
            totalCost += cost * rentDuration * Number(countCurrentEquipment);
        }
    }
   
    return totalCost;
};









