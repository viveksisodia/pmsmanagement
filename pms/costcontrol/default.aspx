<%@ Page Title="Cost Control" Language="C#" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pms_costcontrol_default" %>
<asp:Content ID="CostControlHead" ContentPlaceHolderID="head" runat="Server">
    <script>
        window.CostControlPage = { regid: <%= pageRegId %>, employeeId: <%= pageEmployeeId %>, isadmin: <%= isadmin %> };
    </script>
    <script src="app/costcontrol.main.js?v=<%=DateTime.Now.Ticks %>"></script>
    <style>
        [ng-cloak]{display:none!important}
        .cc-page{padding:37px 2px 14px;background:#f1f5f7;min-height:calc(100vh - 110px);color:#252a31}
        .cc-card{background:#fff;border:1px solid #e3e9ee}
        .cc-titlebar{display:flex;align-items:center;justify-content:space-between;gap:10px;min-height:38px;padding:7px 12px;border-bottom:1px solid #eceef1;flex-wrap:wrap}
        .cc-title{margin:0;font-size:18px;font-weight:600}
        .cc-project{display:flex;align-items:center;gap:8px}
        .cc-project label{margin:0;font-size:12px;color:#5e6672}
        .cc-project select{min-width:280px}
        .cc-page select,.cc-page input,.cc-page textarea{min-height:32px;border:1px solid #d9dde3;border-radius:4px;padding:4px 8px;font-size:12px;background:#fff}
        .cc-page input,.cc-page input:hover{border-bottom:1px solid #d9dde3!important}
        .cc-tabs{display:flex;gap:2px;padding:0 12px;border-bottom:1px solid #eceef1}
        .cc-tab{border:0;background:transparent;padding:9px 14px;font-size:13px;color:#5e6672;cursor:pointer;border-bottom:2px solid transparent}
        .cc-tab.active{color:#7b0b69;border-bottom-color:#7b0b69;font-weight:600}
        .cc-toolbar{display:flex;align-items:center;gap:8px;padding:8px 12px;border-bottom:1px solid #eceef1;flex-wrap:wrap}
        .cc-spacer{flex:1}
        .cc-btn{border:0;border-radius:4px;min-height:32px;padding:5px 12px;font-size:12px;font-weight:600;cursor:pointer;background:#f1f3f5;color:#343a40}
        .cc-btn-primary{background:#7b0b69;color:#fff}
        .cc-btn-danger{background:#fbe9e9;color:#a61b1b}
        .cc-btn:disabled{opacity:.5;cursor:not-allowed}
        .cc-badge{display:inline-block;padding:3px 8px;border-radius:10px;font-size:11px;font-weight:600}
        .cc-badge-draft{background:#fff3cd;color:#7a5d00}
        .cc-badge-locked{background:#dff5e5;color:#23743c}
        .cc-alert{margin:8px 12px 0;padding:8px 10px;border-radius:4px;font-size:12px;background:#fff1f0;border:1px solid #ffd7d4;color:#a61b1b}
        .cc-info{margin:8px 12px 0;padding:8px 10px;border-radius:4px;font-size:12px;background:#eef6ff;border:1px solid #cfe3fb;color:#285ca8}
        .cc-tablewrap{overflow:auto;max-height:calc(100vh - 300px)}
        .cc-table{width:100%;border-collapse:collapse;min-width:900px}
        .cc-table th{position:sticky;top:0;background:#eaf3f7;color:#111;font-size:12px;font-weight:500;padding:6px 7px;border:1px solid #e0eaf1;text-align:left;white-space:nowrap}
        .cc-table td{font-size:12px;padding:5px 7px;border:1px solid #e0eaf1;vertical-align:middle}
        .cc-table tbody tr:nth-child(even){background:#fafbfd}
        .cc-table tfoot td{font-weight:700;background:#f4f7f9}
        .cc-num{text-align:right!important;white-space:nowrap}
        .cc-neg{color:#a61b1b;font-weight:600}
        .cc-empty{padding:28px!important;text-align:center!important;color:#7a828c}
        .cc-actions{white-space:nowrap;width:1%}
        .cc-modal-backdrop{position:fixed;inset:0;background:rgba(0,0,0,.4);z-index:10000;display:flex;align-items:center;justify-content:center}
        .cc-modal{background:#fff;border-radius:8px;padding:18px;width:560px;max-width:95vw;max-height:90vh;overflow:auto}
        .cc-modal h4{margin:0 0 10px;font-size:16px}
        .cc-modal label{display:block;margin:9px 0 3px;font-size:12px;color:#4a525c}
        .cc-modal input,.cc-modal select,.cc-modal textarea{width:100%}
        .cc-grid2{display:grid;grid-template-columns:1fr 1fr;gap:0 12px}
        .cc-grid3{display:grid;grid-template-columns:1fr 1fr 1fr;gap:0 12px}
        .cc-modal-footer{display:flex;gap:8px;justify-content:flex-end;margin-top:14px}
        .cc-quickadd{margin-top:8px;padding:8px 10px;border:1px dashed #b98ab2;border-radius:6px;background:#fbf7fb}
        .cc-hint{font-size:11px;color:#7a828c;margin-top:3px}
        @media(max-width:650px){.cc-project select{min-width:0;width:100%}.cc-grid2,.cc-grid3{grid-template-columns:1fr}}
    </style>
</asp:Content>
<asp:Content ID="CostControlContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cc-page" ng-app="pmsCostControlApp" ng-controller="CostControlController" ng-cloak onkeydown="if(event.keyCode===13 && event.target.tagName!=='TEXTAREA'){event.preventDefault();}">
        <div class="cc-card">
            <div class="cc-titlebar">
                <h3 class="cc-title">Cost Control</h3>
                <div class="cc-project" ng-if="tab!=='codes'">
                    <label for="ccProject">Project</label>
                    <select id="ccProject" ng-model="selection.projectId" ng-options="p.id as p.ProjectName for p in projects" ng-change="projectChanged()">
                        <option value="">-- Select project --</option>
                    </select>
                </div>
            </div>

            <div class="cc-tabs" role="tablist">
                <button type="button" class="cc-tab" ng-class="{active:tab==='budget'}" ng-click="openTab('budget')">Budget</button>
                <button type="button" class="cc-tab" ng-class="{active:tab==='actuals'}" ng-click="openTab('actuals')">Cost Entries</button>
                <button type="button" class="cc-tab" ng-class="{active:tab==='report'}" ng-click="openTab('report')">Budget vs Actual</button>
                <button type="button" class="cc-tab" ng-class="{active:tab==='codes'}" ng-click="openTab('codes')">Cost Codes</button>
            </div>

            <div class="cc-alert" ng-if="error">{{error}}</div>
            <div class="cc-info" ng-if="tab!=='codes' && !selection.projectId">Select a project to continue.</div>
            <div class="cc-info" ng-if="tab!=='codes' && selection.projectId && !costCodes.length">No cost codes yet. Add them in the Cost Codes tab, or choose "+ Add new cost code" in the Cost Code drop-down.</div>

            <!-- ================= BUDGET ================= -->
            <div ng-if="tab==='budget' && selection.projectId">
                <div class="cc-toolbar">
                    <label style="margin:0;font-size:12px">Version</label>
                    <select ng-model="budget.versionId" ng-options="v.id as versionLabel(v) for v in budget.versions" ng-change="loadBudget()" ng-disabled="!budget.versions.length"></select>
                    <span class="cc-badge" ng-if="currentVersion()" ng-class="currentVersion().VersionStatus==='Draft' ? 'cc-badge-draft' : 'cc-badge-locked'">{{currentVersion().VersionStatus}}</span>
                    <span style="font-size:12px;color:#5e6672" ng-if="currentVersion().Reason">Reason: {{currentVersion().Reason}}</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-click="openNewVersion()" ng-disabled="busy || hasDraft()">{{budget.versions.length ? 'New Revision' : 'Create Original Budget'}}</button>
                    <button type="button" class="cc-btn cc-btn-primary" ng-click="openBudgetLine()" ng-disabled="busy || !isDraft()">Add Line</button>
                    <button type="button" class="cc-btn cc-btn-primary" ng-click="lockVersion()" ng-disabled="busy || !isDraft() || !budget.lines.length">Lock Budget</button>
                    <button type="button" class="cc-btn cc-btn-danger" ng-click="discardDraft()" ng-disabled="busy || !isDraft()">Discard Draft</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Cost Code</th><th>Type</th><th>Milestone</th><th>Task</th><th class="cc-num">Qty</th><th>Unit</th><th class="cc-num">Rate</th><th class="cc-num">Amount</th><th>Remarks</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!budget.versions.length"><td colspan="10" class="cc-empty">No budget yet. Click "Create Original Budget".</td></tr>
                            <tr ng-if="budget.versions.length && !budget.lines.length"><td colspan="10" class="cc-empty">No lines in this version.</td></tr>
                            <tr ng-repeat="line in budget.lines track by line.id">
                                <td>{{line.CostCode}} - {{line.CostCodeName}}</td><td>{{line.CostType}}</td>
                                <td>{{line.MilestoneName || 'Project level'}}</td><td>{{line.TaskName}}</td>
                                <td class="cc-num">{{line.Quantity | number:2}}</td><td>{{line.Unit}}</td>
                                <td class="cc-num">{{line.Rate | number:2}}</td><td class="cc-num">{{line.Amount | number:2}}</td>
                                <td>{{line.Remarks}}</td>
                                <td class="cc-actions">
                                    <button type="button" class="cc-btn" ng-if="isDraft()" ng-click="openBudgetLine(line)" title="Edit line">Edit</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="isDraft()" ng-click="deleteBudgetLine(line)" title="Delete line">Delete</button>
                                </td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="budget.lines.length"><tr><td colspan="7">Total</td><td class="cc-num">{{sum(budget.lines,'Amount') | number:2}}</td><td colspan="2"></td></tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= COST ENTRIES ================= -->
            <div ng-if="tab==='actuals' && selection.projectId">
                <div class="cc-toolbar">
                    <input type="search" ng-model="actuals.search" placeholder="Search entries..." aria-label="Search cost entries" style="min-width:240px" />
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn cc-btn-primary" ng-click="openCostEntry()" ng-disabled="busy">Add Cost Entry</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Date</th><th>Cost Code</th><th>Milestone</th><th>Task</th><th class="cc-num">Amount</th><th>Reference</th><th>Remarks</th><th>Source</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!filteredEntries().length"><td colspan="9" class="cc-empty">No cost entries.</td></tr>
                            <tr ng-repeat="entry in filteredEntries() track by entry.id">
                                <td>{{entry.EntryDate}}</td><td>{{entry.CostCode}} - {{entry.CostCodeName}}</td>
                                <td>{{entry.MilestoneName || 'Project level'}}</td><td>{{entry.TaskName}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':entry.Amount<0}">{{entry.Amount | number:2}}</td>
                                <td>{{entry.ReferenceNo}}</td><td>{{entry.Remarks}}</td><td>{{entry.SourceType}}</td>
                                <td class="cc-actions">
                                    <button type="button" class="cc-btn" ng-if="entry.SourceType==='Manual'" ng-click="openCostEntry(entry)">Edit</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="entry.SourceType==='Manual'" ng-click="deleteCostEntry(entry)">Delete</button>
                                </td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="filteredEntries().length"><tr><td colspan="4">Total</td><td class="cc-num">{{sum(filteredEntries(),'Amount') | number:2}}</td><td colspan="4"></td></tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= BUDGET VS ACTUAL ================= -->
            <div ng-if="tab==='report' && selection.projectId">
                <div class="cc-toolbar">
                    <label style="margin:0;font-size:12px">Group by</label>
                    <select ng-model="report.groupBy" ng-change="loadReport()">
                        <option value="COSTCODE">Cost code</option>
                        <option value="MILESTONE">Milestone</option>
                    </select>
                    <span style="font-size:12px;color:#5e6672" ng-if="report.header.CurrentVersionNo">Current budget = locked version {{report.header.CurrentVersionNo}}</span>
                    <span style="font-size:12px;color:#7a5d00" ng-if="report.header.DraftCount>0">A draft revision exists and is not included until it is locked.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-click="loadReport()" ng-disabled="busy">Refresh</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>{{report.groupBy==='MILESTONE' ? 'Milestone' : 'Cost Code'}}</th><th ng-if="report.groupBy!=='MILESTONE'">Type</th><th class="cc-num">Original Budget</th><th class="cc-num">Current Budget</th><th class="cc-num">Actual</th><th class="cc-num">Variance</th><th class="cc-num">Consumed %</th></tr></thead>
                        <tbody>
                            <tr ng-if="!report.rows.length"><td colspan="7" class="cc-empty">No locked budget or cost entries yet.</td></tr>
                            <tr ng-repeat="row in report.rows track by row.GroupId">
                                <td>{{row.GroupName}}</td><td ng-if="report.groupBy!=='MILESTONE'">{{row.CostType}}</td>
                                <td class="cc-num">{{row.OriginalBudget | number:2}}</td><td class="cc-num">{{row.CurrentBudget | number:2}}</td>
                                <td class="cc-num">{{row.Actual | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.Variance<0}">{{row.Variance | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.ConsumedPercent>100}">{{row.ConsumedPercent===null ? '-' : (row.ConsumedPercent | number:2) + ' %'}}</td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="report.rows.length"><tr>
                            <td ng-attr-colspan="{{report.groupBy==='MILESTONE' ? 1 : 2}}">Total</td>
                            <td class="cc-num">{{sum(report.rows,'OriginalBudget') | number:2}}</td><td class="cc-num">{{sum(report.rows,'CurrentBudget') | number:2}}</td>
                            <td class="cc-num">{{sum(report.rows,'Actual') | number:2}}</td>
                            <td class="cc-num" ng-class="{'cc-neg':sum(report.rows,'Variance')<0}">{{sum(report.rows,'Variance') | number:2}}</td>
                            <td class="cc-num">{{totalConsumed()}}</td>
                        </tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= COST CODES ================= -->
            <div ng-if="tab==='codes'">
                <div class="cc-toolbar">
                    <input type="search" ng-model="codes.search" placeholder="Search cost codes..." aria-label="Search cost codes" style="min-width:240px" />
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn cc-btn-primary" ng-click="openCostCode()" ng-disabled="busy">Add Cost Code</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Code</th><th>Name</th><th>Cost Type</th><th>Description</th><th>In Use</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!filteredCodes().length"><td colspan="6" class="cc-empty">No cost codes.</td></tr>
                            <tr ng-repeat="code in filteredCodes() track by code.id">
                                <td>{{code.Code}}</td><td>{{code.Name}}</td><td>{{code.CostType}}</td><td>{{code.Description}}</td>
                                <td>{{code.IsUsed==1 ? 'Yes' : 'No'}}</td>
                                <td class="cc-actions">
                                    <button type="button" class="cc-btn" ng-click="openCostCode(code)">Edit</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-click="deleteCostCode(code)" ng-disabled="code.IsUsed==1" title="{{code.IsUsed==1 ? 'Used in budgets or cost entries' : 'Delete'}}">Delete</button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- ================= MODALS ================= -->
        <div class="cc-modal-backdrop" ng-if="modal.type" ng-keydown="$event.keyCode===27 && closeModal()">
            <div class="cc-modal" role="dialog" aria-modal="true">
                <div class="cc-alert" style="margin:0 0 8px" ng-if="modal.error">{{modal.error}}</div>

                <div ng-if="modal.type==='version'">
                    <h4>{{budget.versions.length ? 'New Budget Revision' : 'Create Original Budget'}}</h4>
                    <div class="cc-hint" ng-if="budget.versions.length">The revision starts as a copy of the last locked version. A reason is required.</div>
                    <label>Reason{{budget.versions.length ? ' *' : ''}}</label>
                    <textarea rows="3" ng-model="modal.data.reason" maxlength="1000"></textarea>
                </div>

                <div ng-if="modal.type==='line'">
                    <h4>{{modal.data.Id ? 'Edit' : 'Add'}} Budget Line</h4>
                    <label>Cost Code *</label>
                    <select ng-model="modal.data.CostCodeId" ng-options="c.id as c.Label for c in costCodeOptions" ng-change="costCodeSelected()"><option value="">-- Select --</option></select>
                    <div class="cc-quickadd" ng-if="quickCode.open"><div class="cc-grid3"><div><label>New Code *</label><input type="text" maxlength="30" ng-model="quickCode.Code" /></div><div><label>Name *</label><input type="text" maxlength="200" ng-model="quickCode.Name" /></div><div><label>Cost Type *</label><select ng-model="quickCode.CostType" ng-options="t for t in costTypes"><option value="">-- Select --</option></select></div></div><div class="cc-alert" style="margin:6px 0 0" ng-if="quickCode.error">{{quickCode.error}}</div><div class="cc-modal-footer" style="margin-top:8px"><button type="button" class="cc-btn" ng-click="cancelQuickCode()" ng-disabled="quickCode.saving">Cancel</button><button type="button" class="cc-btn cc-btn-primary" ng-click="saveQuickCode()" ng-disabled="quickCode.saving">{{quickCode.saving ? 'Adding...' : 'Add Cost Code'}}</button></div></div>
                    <div class="cc-grid2">
                        <div><label>Milestone</label>
                            <select ng-model="modal.data.MilestoneId" ng-options="m.id as m.MilestoneName for m in structure.milestones" ng-change="modal.data.TaskId=null"><option value="">Project level</option></select></div>
                        <div><label>Task</label>
                            <select ng-model="modal.data.TaskId" ng-options="t.id as t.TaskName for t in tasksFor(modal.data.MilestoneId)" ng-disabled="!modal.data.MilestoneId"><option value="">-- None --</option></select></div>
                    </div>
                    <div class="cc-grid3">
                        <div><label>Quantity</label><input type="number" step="0.0001" ng-model="modal.data.Quantity" ng-change="recalculateLineAmount()" /></div>
                        <div><label>Unit</label><input type="text" maxlength="20" ng-model="modal.data.Unit" /></div>
                        <div><label>Rate</label><input type="number" step="0.0001" ng-model="modal.data.Rate" ng-change="recalculateLineAmount()" /></div>
                    </div>
                    <label>Amount *</label>
                    <input type="number" step="0.01" min="0" ng-model="modal.data.Amount" ng-disabled="modal.data.Quantity!=null && modal.data.Rate!=null" />
                    <div class="cc-hint">When quantity and rate are both entered, amount = quantity x rate.</div>
                    <label>Remarks</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Remarks"></textarea>
                </div>

                <div ng-if="modal.type==='entry'">
                    <h4>{{modal.data.Id ? 'Edit' : 'Add'}} Cost Entry</h4>
                    <div class="cc-grid2">
                        <div><label>Date *</label><input type="date" ng-model="modal.data.EntryDateValue" /></div>
                        <div><label>Amount *</label><input type="number" step="0.01" ng-model="modal.data.Amount" /><div class="cc-hint">Use a negative amount for a reversal.</div></div>
                    </div>
                    <label>Cost Code *</label>
                    <select ng-model="modal.data.CostCodeId" ng-options="c.id as c.Label for c in costCodeOptions" ng-change="costCodeSelected()"><option value="">-- Select --</option></select>
                    <div class="cc-quickadd" ng-if="quickCode.open"><div class="cc-grid3"><div><label>New Code *</label><input type="text" maxlength="30" ng-model="quickCode.Code" /></div><div><label>Name *</label><input type="text" maxlength="200" ng-model="quickCode.Name" /></div><div><label>Cost Type *</label><select ng-model="quickCode.CostType" ng-options="t for t in costTypes"><option value="">-- Select --</option></select></div></div><div class="cc-alert" style="margin:6px 0 0" ng-if="quickCode.error">{{quickCode.error}}</div><div class="cc-modal-footer" style="margin-top:8px"><button type="button" class="cc-btn" ng-click="cancelQuickCode()" ng-disabled="quickCode.saving">Cancel</button><button type="button" class="cc-btn cc-btn-primary" ng-click="saveQuickCode()" ng-disabled="quickCode.saving">{{quickCode.saving ? 'Adding...' : 'Add Cost Code'}}</button></div></div>
                    <div class="cc-grid2">
                        <div><label>Milestone</label>
                            <select ng-model="modal.data.MilestoneId" ng-options="m.id as m.MilestoneName for m in structure.milestones" ng-change="modal.data.TaskId=null"><option value="">Project level</option></select></div>
                        <div><label>Task</label>
                            <select ng-model="modal.data.TaskId" ng-options="t.id as t.TaskName for t in tasksFor(modal.data.MilestoneId)" ng-disabled="!modal.data.MilestoneId"><option value="">-- None --</option></select></div>
                    </div>
                    <label>Reference No.</label><input type="text" maxlength="100" ng-model="modal.data.ReferenceNo" placeholder="Bill / voucher / PO number" />
                    <label>Remarks</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Remarks"></textarea>
                </div>

                <div ng-if="modal.type==='code'">
                    <h4>{{modal.data.Id ? 'Edit' : 'Add'}} Cost Code</h4>
                    <div class="cc-grid2">
                        <div><label>Code *</label><input type="text" maxlength="30" ng-model="modal.data.Code" /></div>
                        <div><label>Cost Type *</label><select ng-model="modal.data.CostType" ng-options="t for t in costTypes"><option value="">-- Select --</option></select></div>
                    </div>
                    <label>Name *</label><input type="text" maxlength="200" ng-model="modal.data.Name" />
                    <label>Description</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Description"></textarea>
                </div>

                <div class="cc-modal-footer">
                    <button type="button" class="cc-btn" ng-click="closeModal()" ng-disabled="busy">Cancel</button>
                    <button type="button" class="cc-btn cc-btn-primary" ng-click="saveModal()" ng-disabled="busy">{{busy ? 'Saving...' : 'Save'}}</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
