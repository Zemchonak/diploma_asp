function updateTableByPart(requestUrlPath, aspnetPartParameterName, stringPart,
    noItemsElementText, tableElement, tableBodyElement, tableHeaderElement, colspanAmount, tableRowPrinter) {
    var urlPath = requestUrlPath;
    if (stringPart != '') {
        urlPath += '?' + aspnetPartParameterName + '=';
        urlPath += stringPart;
    }

    var table = tableElement;
    while (table.rows.length > 1)
        table.deleteRow(1);

    $.ajax({
        type: 'GET',
        url: urlPath,
        error: function (error) {
            console.log('Error with receiving data: ' + error)
        },
        success: function (result) {
            let s = '';
            if (result.length == 0) {
                s += '<tr>';
                s += '<td colspan="' + colspanAmount+'" class="text-center">' + noItemsElementText + '</td>';
                s += '</tr>';
            }
            else {
                for (let el of result) {
                    s += tableRowPrinter(el);
                }
            }
            $(tableBodyElement).after(tableHeaderElement);
            $(tableHeaderElement).after(s);
        }
    });
}