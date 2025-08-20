// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function() {
    // Configuración del sidebar
    var sidebarToggle = document.getElementById('sidebarToggle');
    var sidebar = document.getElementById('sidebar');
    var content = document.getElementById('content');
    
    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener('click', function() {
            sidebar.classList.toggle('collapsed');
            if (content) content.classList.toggle('expanded');
            
            // Guardar el estado del sidebar en localStorage
            var isCollapsed = sidebar.classList.contains('collapsed');
            localStorage.setItem('sidebarCollapsed', isCollapsed ? 'true' : 'false');
        });
    }
    
    // Restaurar el estado del sidebar desde localStorage
    function restoreSidebarState() {
        var isCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
        
        if (isCollapsed && window.innerWidth >= 992) {
            sidebar.classList.add('collapsed');
            content.classList.add('expanded');
        } else {
            sidebar.classList.remove('collapsed');
            content.classList.remove('expanded');
        }
    }
    
    // Inicializar el estado del sidebar
    restoreSidebarState();
    
    // Ajustar en cambios de tamaño
    window.addEventListener('resize', function() {
        const isSmallScreen = window.innerWidth < 992;
        
        if (isSmallScreen && sidebar && !sidebar.classList.contains('collapsed')) {
            sidebar.classList.add('collapsed');
            if (content) content.classList.add('expanded');
        } else if (!isSmallScreen) {
            restoreSidebarState();
        }
    });
    
    // Configuración del autocompletado
    $(function() {
        if ($("#search-tasks").length > 0) {
            $("#search-tasks").autocomplete({
                source: function(request, response) {
                    $.ajax({
                        url: "/api/tasks/search",
                        method: "GET",
                        data: { term: request.term },
                        success: function(data) {
                            response(data.map(function(item) {
                                return {
                                    label: item.nombre + (item.descripcion ? ' - ' + item.descripcion : ''),
                                    value: item.nombre,
                                    id: item.id,
                                    estado: item.estado
                                };
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function(event, ui) {
                    if (ui.item) {
                        window.location.href = "/TaskDetail?id=" + ui.item.id + "&returnPage=/Index";
                    }
                    return false;
                },
                focus: function(event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.value);
                }
            }).autocomplete("instance")._renderItem = function(ul, item) {
                return $("<li>")
                    .append("<div class='autocomplete-item'>" +
                        "<div class='task-name'>" + item.value + "</div>" +
                        "<span class='badge " + getStatusBadgeClass(item.estado) + "'>" + 
                        item.estado + "</span></div>")
                    .appendTo(ul);
            };
        }
    });
    
    // Función auxiliar para determinar la clase CSS del badge según el estado
    function getStatusBadgeClass(estado) {
        switch(estado.toLowerCase()) {
            case 'completada':
                return 'bg-success';
            case 'cancelada':
                return 'bg-danger';
            default:
                return 'bg-primary';
        }
    }
    
    // Inicializar tooltips de Bootstrap
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    if (typeof bootstrap !== 'undefined') {
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl)
        });
    }
    
    // Inicializar dropdowns de Bootstrap
    var dropdownTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="dropdown"]'))
    if (typeof bootstrap !== 'undefined') {
        dropdownTriggerList.map(function (dropdownTriggerEl) {
            return new bootstrap.Dropdown(dropdownTriggerEl)
        });
    }
    
    // Inicializar collapses de Bootstrap para el sidebar
    var collapseTriggerList = [].slice.call(document.querySelectorAll('.sidebar-dropdown-toggle'))
    if (typeof bootstrap !== 'undefined') {
        collapseTriggerList.forEach(function (collapseTriggerEl) {
            collapseTriggerEl.addEventListener('click', function(e) {
                e.preventDefault();
                var targetId = this.getAttribute('href');
                var targetCollapse = document.querySelector(targetId);
                if (targetCollapse && typeof bootstrap.Collapse !== 'undefined') {
                    new bootstrap.Collapse(targetCollapse, {
                        toggle: true
                    });
                }
            });
        });
    }
});
