var stepCurrent=1;

/*********************************合同查询功能 Start*********************************/

//判断输入的查询条件
function decideInputCondition(){
    var m_ConCode = $("#txtConCode").val();
    var m_CustomerName = $("#txtCustomerName").val();
    //var m_OwnerName = $("#txtOwnerName").val();
    var m_Phone = $("#txtPhone").val();
    if (m_ConCode == "" || m_ConCode == undefined) {
        //alert("合同编号为必填项");
        $("#alertinfo").removeClass("alert alert-success");
        $("#alertinfo").text("合同编号为必填项");
        $("#alertinfo").addClass("alert alert-danger");
        return false;
    }

    if ((m_CustomerName == "" || m_CustomerName == undefined) &&
        (m_Phone == "" || m_Phone == undefined)) {
        //alert("对不起,您必须填写一个辅助条件,才具备查询合同的条件");
        $("#alertinfo").removeClass("alert alert-success");
        $("#alertinfo").text("对不起,您必须填写一个辅助条件");
        $("#alertinfo").addClass("alert alert-danger");
        return false;
    }
    else {
        return true;
    }
}

function JumpToResult() {
    if (decideInputCondition() == false) {
        return;
    }
    setCookie("ConCode", $("#txtConCode").val());
    setCookie("CustomerName", $("#txtCustomerName").val());
    window.location.href = encodeURI("Contract_TP.html?ConCode=" + $("#txtConCode").val() + "&CustomerName=" + $("#txtCustomerName").val());
    
}

//返回合同结果
function returnContractResult() {
    $.ajax({
        type: "POST",
        url: "../../ashx/mfr_Contract.ashx",
        data: { "action": "ContractQuery", "ConCode": getQueryString("ConCode"), "CustomerName":getCookie("CustomerName")},
        dataType: "text",
        success: function (str) {
            //alert(str);
            msg = jQuery.parseJSON(str);
            var imgarr = [];
            var imgarr = [];
            imgarr[0] = "<img src='../../images/02.png'  style='padding-top:2px;'>";
            imgarr[1] = "<img src='../../images/03.png' style='padding-top:2px;'>";
            imgarr[2] = "<img src='../../images/04.png' style='padding-top:2px;'>";
            imgarr[3] = "<img src='../../images/05.png' style='padding-top:4px;'>";
            imgarr[4] = "<img src='../../images/06.png' >";
            imgarr[5] = "<img src='../../images/07.png' >";
            imgarr[6] = "<img src='../../images/08.png' >";
            imgarr[7] = "<img src='../../images/09.png' >";
            imgarr[8] = "<img src='../../images/10.png' >";
            var qz = "";
            var headItem = "<div>签约人:(" + msg.签约部门 + ")" + msg.签约人 + "</div><div>负责人：(" + msg.签约负责人部门 + ")" + msg.签约负责人 + "</div>";
            var lcItem = " <div class='step step1'>过户手续流程</div>";
            var lcProcess = "<p class='ui-stepInfo'><a class='ui-stepSequence'><img src='../../images/01.png' ></a></p>";
            for (var i = 0; i < msg.transfer.length; i++) {
                //alert(msg.transfer[i].name);
                qz = msg.transfer[i].name;

                if (msg.transfer[i].node.length > 0) {
                    for (var j = 0; j < msg.transfer[i].node.length; j++) {
                        //alert(msg.transfer[i].node[j].环节名)
                        if (msg.transfer[i].node[j].完成日期 != "null") {
                            lcItem = lcItem + "<div class='step'>[&nbsp;" + qz + "&nbsp;]" + msg.transfer[i].node[j].环节名 + "已完成<p>经办人:" + msg.transfer[i].node[j].完成人 + "</p><p>" + msg.transfer[i].node[j].完成日期 + "</p></div>";
                            stepCurrent++;
                            //alert(1);
                        } else {
                            var getdate = "";
                            if (msg.transfer[i].node[j].完成日期 != "null") getdate = msg.transfer[i].node[j].完成日期;
                            lcItem = lcItem + "<div class='step'>[&nbsp;" + qz + "&nbsp;]" + msg.transfer[i].node[j].环节名 + "未完成<p>" + getdate + "</p></div>";
                            //alert(0);
                        }
                        lcProcess = lcProcess + "<p class='ui-stepInfo'><a class='ui-stepSequence'>" + imgarr[i] + "</a></p>";
                    }
                }
            }
            
            lcItem = lcItem + "</div>";
            $("#ui_step_img_div").html(lcProcess);
            $("#headItem").html(headItem);
            $("#ui_step_text_div").html(lcItem);
            stepBar.init("stepBar", {
                step: stepCurrent,
                //step: 4,
                change: true,
                animation: true
            });
        }
    });
}
function returnContractResult1() {
    
        //var str = '{"htNo": "NF1505579","flag": "过户信息返回成功","transfer": [{ "name": "备件", "node": [{"环节名": "接单回访", "完成人": "小米", "完成日期": "2015-01-01" }, { "环节名": "买方材料","完成人": "null", "完成日期": "null"}, {"环节名": "卖方材料", "完成人": "null","完成日期": "null" }] }, {"name": "过户","node": [ { "环节名": "过户受理", "完成人": "null", "完成日期": "null"},{ "环节名": "委托公证", "完成人": "null", "完成日期": "null" } ] }, { "name": "权证", "node": [{"环节名": "打单", "完成人": "null", "完成日期": "null" },{ "环节名": "缴费", "完成人": "null","完成日期": "null" },{"环节名": "领证","完成人": "null","完成日期": "null" } ]},{"name": "交接","node": [{"环节名": "房屋交接", "完成人": "null", "完成日期": "null"} ]} ]}';
        var str = '{"htNo":"NF1505284","flag":"过户信息返回成功","transfer":[{"name":"备件","node":[{"环节名":"接单回访","完成人":"xiao","完成日期":"2011-01-01"},{"环节名":"买方材料","完成人":"bbc","完成日期":"2014-01-01"},{"环节名":"卖方材料","完成人":"null","完成日期":"null"}]},{"name":"贷款","node":[{"环节名":"贷款受理","完成人":"null","完成日期":"null"},{"环节名":"跟踪","完成人":"null","完成日期":"null"},{"环节名":"批贷","完成人":"null","完成日期":"null"},{"环节名":"交件","完成人":"null","完成日期":"null"},{"环节名":"办理抵押","完成人":"null","完成日期":"null"},{"环节名":"领他项","完成人":"null","完成日期":"null"},{"环节名":"批贷","完成人":"null","完成日期":"null"},{"环节名":"抵押","完成人":"null","完成日期":"null"},{"环节名":"1","完成人":"null","完成日期":"null"}]},{"name":"过户","node":[{"环节名":"过户受理","完成人":"null","完成日期":"null"},{"环节名":"委托公证","完成人":"null","完成日期":"null"}]},{"name":"权证","node":[{"环节名":"打单","完成人":"null","完成日期":"null"},{"环节名":"缴费","完成人":"null","完成日期":"null"},{"环节名":"领证","完成人":"null","完成日期":"null"}]},{"name":"交接","node":[{"环节名":"房屋交接","完成人":"null","完成日期":"null"}]}]}';
        msg = jQuery.parseJSON(str);
       
        var imgarr = [];
        imgarr[0] = "<img src='../../images/02.png'  style='padding-top:2px;'>";
        imgarr[1] = "<img src='../../images/03.png' style='padding-top:2px;'>";
        imgarr[2] = "<img src='../../images/04.png' style='padding-top:2px;'>";
        imgarr[3] = "<img src='../../images/05.png' style='padding-top:4px;'>";
        imgarr[4] = "<img src='../../images/06.png' >";
        imgarr[5] = "<img src='../../images/07.png' >";
        imgarr[6] = "<img src='../../images/08.png' >";
        imgarr[7] = "<img src='../../images/09.png' >";
        imgarr[8] = "<img src='../../images/10.png' >";
        var qz = "";
        var lcItem = " <div class='step step1'>过户手续流程</div>";
        var lcProcess = "<p class='ui-stepInfo'><a class='ui-stepSequence'><img src='../../images/01.png' ></a></p>";
        
        for (var i = 0; i < msg.transfer.length; i++) {
            //alert(msg.transfer[i].name);
            qz = msg.transfer[i].name;
            if (msg.transfer[i].node.length > 0) {
                for (var j = 0; j < msg.transfer[i].node.length; j++) {
                    //alert(msg.transfer[i].node[j].环节名)
                    if (msg.transfer[i].node[j].完成人 != "null") {
                        lcItem = lcItem + "<div class='step'>［" + qz + "］" + msg.transfer[i].node[j].环节名 + "已完成<p>" + msg.transfer[i].node[j].完成日期 + "</p></div>";
                        stepCurrent++;
                    } else {
                        var getdate="";
                        if (msg.transfer[i].node[j].完成日期 != "null") getdate = msg.transfer[i].node[j].完成日期;
                        lcItem = lcItem + "<div class='step'>［" + qz + "］" + msg.transfer[i].node[j].环节名 + "未完成<p>" + getdate + "</p></div>";
                    }
                    lcProcess = lcProcess + "<p class='ui-stepInfo'><a class='ui-stepSequence'>" + imgarr[i] + "</a></p>";
                }
            }
        }
        lcItem = lcItem + "</div>";
        $("#ui_step_img_div").html(lcProcess);
        $("#ui_step_text_div").html(lcItem);
        stepBar.init("stepBar", {
            step: stepCurrent,
            //step: 4,
            change: true,
            animation: true
        });
       // $("#stepBar").append(lcProcess);
       // $("#process-right").html(lcItem);
        //设置当前的流到哪一步
        
}
//返回测试合同结果
function returnTestContractResult() {
    var msg = "{'htNo': 'NF1505284','customer': '刘海燕','owner': '','flag': '过户信息返回成功','transfer': [{'name': '备件','node': [{'jdhf':'接单回访', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'买方材料', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'卖方材料', 'jdhf':'null', 'jdhf':'null'}]}, {'name': '贷款','node': [{'jdhf':'贷款受理', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'跟踪', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'批贷', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'交件', 'jdhf':'null', 'jdhf':'null'},{'jdhf':'办理抵押', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'领他项', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'批贷', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'抵押', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'1', 'jdhf':'null', 'jdhf':'null'}]}, {'name': '过户','node': [{'jdhf':'过户受理', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'委托公证', 'jdhf':'null', 'jdhf':'null'}]}, {'name': '权证','node': [{'jdhf':'打单', 'jdhf':'null', 'jdhf':'null'}, {'jdhf':'缴费', 'jdhf':'null', 'jdhf':'null'},{'jdhf':'领证', 'jdhf':'null', 'jdhf':'null'}]}, {'name': '交接','node': [{'jdhf':'房屋交接', 'jdhf':'null', 'jdhf':'null'}]}]}";
    JSON.parse(msg);
    alert(msg.htNo);
    $("#process-right").text(msg.replace("'", "\""));
}
/*********************************合同查询功能 End*********************************/

/*************************** --公共验证使用的js Start-- *****************************/
//手机号码验证
function validatemobile(mobile) {
    if (mobile.length == 0) {
        alert('请输入手机号码！');
        return false;
    }
    if (mobile.length != 11) {
        alert('请输入有效的手机号码！');
        return false;
    }
    //可验证153（联通）,159（移动）等新区段的开通
    var myreg = /^(((13[0-9]{1})|159|153)+\d{8})$/;
    if (!myreg.test(mobile)) {
        alert('请输入有效的手机号码！');
        return false;
    }
}

//电子邮箱的验证
function validateemail(email) {
    //对电子邮件的验证
    var myreg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
    if (!myreg.test(email)) {
        alert('提示\n\n请输入有效的E_mail！');
        return false;
    }
}

//验证密码
function validatepassword(pwd) {
    var test = /^[a-zA-Z0-9]+$/;              //正则表达式
    if (test.exec(pwd))                            //exec(string) 为验证字符串是否符合正则表达式
    {
        alert("对不起,只能输入8-16含有特殊符号字母数字的密码");
        return false;
    }
}

//js获取URL中的参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

function GetRequestString() {
    var urlinfo = window.location.href;//获取url
    alert(urlinfo);
    var userName = urlinfo.split("?")[1].split("=")[1];//拆分url得到”=”后面的参数
    return decodeURI(userName);
}
/*************************** --公共验证使用的js End-- *****************************/

/*************************** --公共操作的Cookies Start-- *****************************/
//写cookies
function setCookie(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

//读取cookies
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

    if (arr = document.cookie.match(reg))

        return unescape(arr[2]);
    else
        return null;
}

//删除cookies
function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}
/*************************** --公共操作的Cookies Start-- *****************************/