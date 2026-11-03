(function(){'use strict';angular.module('pmsDashboardReportApp',[]).factory('ReportService',['$http',function($http){
function call(name,data){return $http.post('default.aspx/'+name,data||{}).then(function(r){return r.data.d;});}
        function localDate(d) {
            return d.getFullYear() + '-' + ('0' + (d.getMonth()+1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);
        }
        function dateRange(preset, from, to, now) {
            var today = now || new Date(), y=today.getFullYear(), m=today.getMonth(), d=today.getDate();
            var start=new Date(y,m,d), end=new Date(y,m,d), week=(today.getDay()+6)%7;
            switch(preset) {
                case 'All': start=null; end=null; break;
                case 'Yesterday': start=new Date(y,m,d-1); end=new Date(y,m,d-1); break;
                case 'This Week': start=new Date(y,m,d-week); end=new Date(y,m,d-week+6); break;
                case 'Previous Week': start=new Date(y,m,d-week-7); end=new Date(y,m,d-week-1); break;
                case 'This Month': start=new Date(y,m,1); end=new Date(y,m+1,0); break;
                case 'Previous Month': start=new Date(y,m-1,1); end=new Date(y,m,0); break;
                case 'This Quarter': start=new Date(y,Math.floor(m/3)*3,1); end=new Date(y,Math.floor(m/3)*3+3,0); break;
                case 'Previous Quarter': start=new Date(y,Math.floor(m/3)*3-3,1); end=new Date(y,Math.floor(m/3)*3,0); break;
                case 'This Year': start=new Date(y,0,1); end=new Date(y,11,31); break;
                case 'Previous Year': start=new Date(y-1,0,1); end=new Date(y-1,11,31); break;
                case 'Custom':
                    if (!(from instanceof Date) || !(to instanceof Date) || isNaN(from.getTime()) || isNaN(to.getTime())) throw new Error('Select From Date and To Date.');
                    start=new Date(from.getFullYear(),from.getMonth(),from.getDate());
                    end=new Date(to.getFullYear(),to.getMonth(),to.getDate());
                    if(start>end) throw new Error('From Date must be on or before To Date.');
            }
            var zone=null;
            try { zone=Intl.DateTimeFormat().resolvedOptions().timeZone; } catch(e) {}
            return { startDate:start?localDate(start):null, endDate:end?localDate(end):null,
                timeZone:zone, utcOffsetMinutes:-today.getTimezoneOffset(), preset:preset };
        }
return {range:dateRange,options:function(){return call('GetOptions');},report:function(f){return call('GetReport',f);}};}]);}());
