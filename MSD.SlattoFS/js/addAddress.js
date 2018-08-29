function addAddress() {
    var newAddressDiv = document.createElement("DIV");
    var addressLine = document.createElement("input");

    var address1Input = document.createElement("input");
    var address2Input = document.createElement("input");
    var cityInput = document.createElement("input");
    var stateInput = document.createElement("input");
    var countryInput = document.createElement("input");
    var zipInput = document.createElement("input");

    var address1Label = document.createElement("label").appendChild(document.createTextNode("Address 1: "));
    var address2Label = document.createElement("label").appendChild(document.createTextNode("Address 2: "));
    var cityLabel = document.createElement("label").appendChild(document.createTextNode("City: "));
    var stateLabel = document.createElement("label").appendChild(document.createTextNode("State/Province: "));
    var countryLabel = document.createElement("label").appendChild(document.createTextNode("Country: "));
    var zipLabel = document.createElement("label").appendChild(document.createTextNode("Zipcode"));

    newAddressDiv.name = "newAddressLineDIV";
    addressLine.name = "newAddressLine";
    addressLine.hidden = "hidden";
    addressLine.value = "New Address";

    address1Input.name = "newAddress1";
    address1Input.className = "required form-control";
    address1Input.placeholder = "(Street address, P.O. box, company name, c/o )";
    address2Input.name = "newAddress2";
    address2Input.className = "form-control";
    address2Input.placeholder = "(Building, unit, apt., floor, etc.)";
    cityInput.name = "newCity";
    cityInput.className = "required form-control";
    stateInput.name = "newState";
    stateInput.className = "required form-control";
    countryInput.name = "newCountry";
    countryInput.className = "required form-control";
    zipInput.name = "newZipCode";
    zipInput.className = "required form-control";

    newAddressDiv.appendChild(document.createElement("hr")); // line
    newAddressDiv.className = "address-row"; //alternating color from address-style.css

    newAddressDiv.appendChild(addressLine);
    newAddressDiv.appendChild(address1Label);
    newAddressDiv.appendChild(address1Input);
    newAddressDiv.appendChild(address2Label);
    newAddressDiv.appendChild(address2Input);
    newAddressDiv.appendChild(cityLabel);
    newAddressDiv.appendChild(cityInput);
    newAddressDiv.appendChild(stateLabel);
    newAddressDiv.appendChild(stateInput);
    newAddressDiv.appendChild(countryLabel);
    newAddressDiv.appendChild(countryInput);
    newAddressDiv.appendChild(zipLabel);
    newAddressDiv.appendChild(zipInput);

    var cancelButton = document.createElement("button");
    cancelButton.innerHTML = "Cancel";
    cancelButton.className = "cancel-address btn btn-danger";
    cancelButton.href = "javascript:cancelAddress();";
    cancelButton.type = "button";
    newAddressDiv.appendChild(cancelButton);

    document.getElementById("newAddress").appendChild(newAddressDiv);
        
    $(".cancel-address").click(function () {
        $(this).parent("div").find('input:text').val('');
        $(this).parent("div").hide();
    });
}
