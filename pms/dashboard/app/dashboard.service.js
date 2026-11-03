(function () {
    'use strict';
    // Issues and tags keep the supplied static design.
    angular.module('pmsManagerDashboardApp').factory('DashboardService', ['$http', function ($http) {
        function getChartDefinitions() {
            var definitions = [];
            function addChart(id, config) { definitions.push({ id: id, config: config }); }
        const colors = ['#f7c948', '#f4a582', '#a5b4fc', '#607d8b', '#4ade80'];

        const doughnutOptions = (number, label) => ({
            responsive: true,
            maintainAspectRatio: false,
            cutout: '82%',
            plugins: {
                legend: { display: false },
                centerText: { number, label },
                tooltip: { callbacks: { label: c => `${c.raw} ${c.label}` } }
            }
        });

        // DOUGHNUT CHARTS
        addChart('projectChart', {
            type: 'doughnut',
            data: { labels: [], datasets: [{ data: [], backgroundColor: colors, borderWidth: 0, radius: '90%' }] },
            options: doughnutOptions(0, 'Total Projects')
        });
        addChart('milestoneChart', {
            type: 'doughnut',
            data: { labels: ['To Start', 'In Progress', 'Ready', 'Hold', 'Completed'], datasets: [{ data: [], backgroundColor: colors, borderWidth: 0, radius: '90%' }] },
            options: doughnutOptions(0, 'Total Milestones')
        });
        addChart('taskChart', {
            type: 'doughnut',
            data: { labels: ['To Start', 'In Progress', 'Ready', 'Hold', 'Completed'], datasets: [{ data: [], backgroundColor: colors, borderWidth: 0, radius: '90%' }] },
            options: doughnutOptions(0, 'Total Task')
        });
        addChart('subtaskChart', {
            type: 'doughnut',
            data: { labels: ['To Start', 'In Progress', 'Ready', 'Hold', 'Completed'], datasets: [{ data: [], backgroundColor: colors, borderWidth: 0, radius: '90%' }] },
            options: doughnutOptions(0, 'Total Subtask')
        });

        // BAR CHART
        addChart('barChart', {
            type: 'bar',
            data: { labels: ['0%-25%', '>25%-50%', '>50%-75%', '>75%-100%'], datasets: [{ label: 'Projects', data: [0, 0, 0, 0], backgroundColor: '#f4b6e5', borderRadius: 0, barThickness: 30 }] },
            options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { display: false } }, scales: { y: { beginAtZero: true, grid: { color: '#e5e7eb' } }, x: { grid: { display: false } } } }
        });
        // ISSUES CHART
        addChart('issuesChart', {
            type: 'doughnut',
            data: { labels: ['Open', 'Close'], datasets: [{ data: [0, 0], backgroundColor: colors, borderWidth: 0, radius: '90%' }] },
            options: doughnutOptions(0, 'Total Issues')
        });
        // PRIORITY CHART FIXED
        const priorityCtx = 'priorityChart';
        addChart(priorityCtx, {
            type: 'bar',
            data: {
                labels: [],
                datasets: [
                    { label: 'High', data: [], backgroundColor: 'rgba(255,99,132,0.6)', barThickness: 20 },
                    { label: 'Medium', data: [], backgroundColor: 'rgba(54,162,235,0.6)', barThickness: 20 },
                    { label: 'Low', data: [], backgroundColor: 'rgba(255,206,86,0.6)', barThickness: 20 }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                layout: { padding: 0 },
                interaction: { mode: 'index', intersect: false },
                animation: { duration: 1200 },
                plugins: {
                    legend: {
                        display: true,
                        position: 'top',
                        labels: {
                            usePointStyle: true,
                            pointStyle: 'circle',

                            // custom box size (this is the key)
                            boxWidth: 8,
                            boxHeight: 8,

                            padding: 10,

                            generateLabels: (chart) => {
                                return chart.data.datasets.map((dataset, i) => ({
                                    text: dataset.label,
                                    fillStyle: dataset.backgroundColor,
                                    strokeStyle: 'transparent',   // ❌ no border
                                    lineWidth: 0,                 // ❌ no border
                                    hidden: !chart.isDatasetVisible(i),
                                    index: i,
                                    pointStyle: 'circle'
                                }));
                            }
                        }
                    }
                },
                scales: {
                    y: { stacked: false, beginAtZero: true, suggestedMax: 150, grid: { color: '#fff' } },
                    x: { stacked: false, grid: { display: false } }
                }
            }

        });
    
            definitions.forEach(function(definition){definition.config.options.animation={duration:250};});
            return definitions;
        }
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
        function getProjects(filters) {
            return $http.post('default.aspx/GetProjects', filters, {headers:{'Content-Type':'application/json; charset=utf-8'}}).then(function(response) { return response.data.d; });
        }
        return { getProjects:getProjects, getChartDefinitions: getChartDefinitions, dateRange:dateRange, getEmployees:function() {
            return $http.post('default.aspx/GetEmployees', {}, {headers:{'Content-Type':'application/json; charset=utf-8'}}).then(function(response) { return response.data.d; });
        } };
    }]);
}());
