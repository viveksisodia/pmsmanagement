<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="projectplaning.aspx.cs" Inherits="settings_projectplaning" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
    .setting-table{
    width:100%;
    max-width:550px;
    border:1px solid #e5e5e5;
    border-radius:6px;
    overflow:hidden;
    margin-top:15px;
}

.setting-row{
    display:grid;
    grid-template-columns:1fr 90px;
    align-items:center;
    min-height:48px;
    border-bottom:1px solid #ececec;
}

.setting-row:last-child{
    border-bottom:none;
}

.setting-label{
    padding:12px 18px;
    font-size:15px;
    font-weight:600;
}

.setting-value{
    display:flex;
    justify-content:center;
    align-items:center;
    padding:8px;
    border-left:1px solid #ececec;
}
.switch{
    position:relative;
    width:46px;
    height:24px;
    display:inline-block;
}

.switch input{
    display:none;
}

.slider{
    position:absolute;
    inset:0;
    background:#d7d7d7;
    border-radius:50px;
    cursor:pointer;
    transition:.3s;
}

.slider:before{
    content:"";
    position:absolute;
    width:18px;
    height:18px;
    left:3px;
    top:3px;
    background:#fff;
    border-radius:50%;
    transition:.3s;
    box-shadow:0 1px 4px rgba(0,0,0,.25);
}

.switch input:checked + .slider{
    background:#28a745;
}

.switch input:checked + .slider:before{
    transform:translateX(22px);
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="setting-table">

    <div class="setting-row">
        <div class="setting-label">Calculate Holidays</div>
        <div class="setting-value">
            <label class="switch">
                <input type="checkbox" id="countHoliday">
                <span class="slider"></span>
            </label>
        </div>
    </div>

    <div class="setting-row">
        <div class="setting-label">Calculate Weekoff</div>
        <div class="setting-value">
            <label class="switch">
                <input type="checkbox" id="countWeekoff">
                <span class="slider"></span>
            </label>
        </div>
    </div>

</div>
    
    <script>
const checkboxholiday = document.getElementById("countHoliday");
const checkboxweekoff = document.getElementById("countWeekoff");
checkboxholiday.addEventListener("change", function () {
    //text.textContent = this.checked ? "Yes" : "No";
        });
        checkboxweekoff.addEventListener("change", function () {
    
        });

    </script>
</asp:Content>

