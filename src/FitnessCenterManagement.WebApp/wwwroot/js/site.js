function getTableTexts() {
    var items = [
        "lengthMenuText",
        "zeroRecordText",
        "infoText",
        "infoEmptyText",
        "infoFilteredText",
        "emptyTableText",
        "loadingText",
        "processingText",
        "searchText",
        "firstText",
        "lastText",
        "nextText",
        "previousText",
        "ascText",
        "descText",
    ]

    var texts = [];
    for(let one of items){
        texts.push(
            document.getElementById(one).innerText
        );
        document.getElementById(one).remove();
    }
    return texts;
}