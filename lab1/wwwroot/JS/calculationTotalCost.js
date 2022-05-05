$(document).on('change', '.SelectEquipments input[type=checkbox]', function () {

    
    let rentDuration = getDurationRent();
    let selectBox = this.parentNode.childNodes[1];
    let cost = selectBox.value;

    let infoTotalCost = document.getElementById("totalCostValue");
    if (this.checked) {
        
        infoTotalCost.textContent = (Number(cost) * rentDuration) + Number(infoTotalCost.textContent);
    }
    else {
        infoTotalCost.textContent = Number(infoTotalCost.textContent) - (Number(cost) * rentDuration);
    }

}); 

$(document).on('change', '#SelectServices input[type=checkbox]', function () {

    let selectBox = this.parentNode.childNodes[1];
    let cost = selectBox.value;
    let infoTotalCost = document.getElementById("totalCostValue");
    if (this.checked) {

        infoTotalCost.textContent = Number(cost) + Number(infoTotalCost.textContent);
    }
    else {
        infoTotalCost.textContent = Number(infoTotalCost.textContent) - Number(cost);
    }
    
});

$(document).on('change', 'input[type=date],.time-select', function () {

    document.getElementById("totalCostValue").textContent = calculationTotalCost();
});

$(document).on('change', '#HallSelect', function () {
    
    document.getElementById("totalCostValue").textContent = calculationTotalCost();
});

function calculationTotalCost() {

    let rentDuration = getDurationRent();

    let hallSelectedBox = document.getElementById("HallSelect");
    let hallPriceBox = document.getElementById("hallInfo");
    let selectHallsPrice = 0;

    for (let item of hallPriceBox.childNodes) {
        if (item.nodeName.toLowerCase() == "option") {
            if (Number(item.value) === Number(hallSelectedBox.value)) {
                selectHallsPrice = Number(item.textContent);
            }
        }
    }

    let totalCost = selectHallsPrice * rentDuration;
    let checkboxCollection = $('input[type="checkbox"]');

    for (item of checkboxCollection) {
        if (item.checked && item.name === "SelectedServices") {
            let cost = Number(item.parentNode.childNodes[1].value);
            totalCost += cost;
        }
        else if (item.checked && item.name === "SelectedEquipments") {
            let cost = Number(item.parentNode.childNodes[1].value);
            totalCost += cost * rentDuration;
        }
    }
    return totalCost;
}

function getDurationRent() {
    let boxStartDateRent = document.getElementById("startDateHallsRent");
    let boxStartTimeRent = document.getElementById("startTimeHallsRent");

    let boxEndDateRent = document.getElementById("endDateHallsRent");
    let boxEndTimeRent = document.getElementById("endTimeHallsRent");

    let startDateTimeRent = new Date(new Date(boxStartDateRent.value).setHours(Number(boxStartTimeRent.value)));
    let endDateTimeRent = new Date(new Date(boxEndDateRent.value).setHours(Number(boxEndTimeRent.value)));
    let rentDuration = (endDateTimeRent - startDateTimeRent) / (3600 * 1000);
    return rentDuration;
}