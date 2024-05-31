
function animateScroll($elment) {
    var itop = $($elment).position().top;
    $(".content-middle-box").stop().animate({
        scrollTop: itop+200
    }, 200);
}

$(document).on('click', 'span.json-property', function () {
    var $element = $($(this).attr('href'));
    $('tr.ant-table-row').removeClass('active');
    $element.addClass('active');
    window.location.href = $(this).attr('href');
})

var jsonmodel = parent.paramjson;
$('#columnId').text(jsonmodel.columnId);
$('#page').text(jsonmodel.page);
$('#pagesize').text(jsonmodel.pagesize);
$('#modelId').text(jsonmodel.modelId);
$('#PId').text(jsonmodel.PId);
$('#kw').text(jsonmodel.k);

ajaxHttp({
    url: '/api/JsonDocx/GetJsonDocxContent'
    , type: 'Get'
    , data: jsonmodel
    , dataType: 'json'
    , success: function (json) {
        if (json.code == 200) {
            $('.json-editor-blackbord').html(json.content.JsonStr);
            var data = json.content.full_fileds;
            $('#apilink').attr('href', json.content.urlext);
            $('#spanapilink').text(json.content.urlext);
            var htmlstr = '';
            if (data.length <= 0)
                return false;
            $.each(data, function (key, value) {
                var tbname = key.split('：')[0];
                htmlstr += "<div class=\"css-npueup\">";
                htmlstr += "<h4 class=\"apipost-table-title\" id='" + tbname +"'>" + key + "</h4>";
                htmlstr += "<table style=\"table-layout: auto;\">";
                htmlstr += "<colgroup></colgroup>";
                htmlstr += "<thead class=\"ant-table-thead\">";
                htmlstr += "<tr>";
                htmlstr += "<th class=\"ant-table-cell\" scope=\"col\">参数名</th>";
                htmlstr += "<th class=\"ant-table-cell\" scope=\"col\">字段名</th>";
                htmlstr += "<th class=\"ant-table-cell\" scope=\"col\">参数类型</th>";
                htmlstr += "<th class=\"ant-table-cell\" scope=\"col\">参与搜索</th>";
                htmlstr += "</tr>";
                htmlstr += "</thead>";
                htmlstr += "<tbody class=\"ant-table-tbody\">";
                $.each(value, function (index, item) {
                    htmlstr += "<tr data-row-key=\"" + item.FiledName + "\" class=\"ant-table-row ant-table-row-level-0\" id=\"" + tbname + item.FiledName +"\">";
                    htmlstr += "<td class=\"ant-table-cell\">";
                    htmlstr += "<div style=\"min-width: 100px;\">" + item.FiledName +"</div>";
                    htmlstr += "</td>";
                    htmlstr += "<td class=\"ant-table-cell\">";
                    htmlstr += "<div style=\"min-width: 100px;\">";
                    htmlstr += "<div>";
                    htmlstr += "<div>" + item.FiledDesc +"</div>";
                    htmlstr += "</div>";
                    htmlstr += "</div>";
                    htmlstr += "</td>";
                    htmlstr += "<td class=\"ant-table-cell\">";
                    htmlstr += "<div style=\"min-width: 100px;\">" + item.FiledMode +"</div>";
                    htmlstr += "</td>";
                    htmlstr += "<td class=\"ant-table-cell\">";
                    htmlstr += "<div style=\"min-width: 100px;\">" + item.IsSearch +"</div>";
                    htmlstr += "</td>";
                    htmlstr += "</tr>";
                });
                htmlstr += "</tbody>";
                htmlstr += "</table>";
                htmlstr += "</div>";
            })
            $('#filedlist').html(htmlstr);
        }
    }
})