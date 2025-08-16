// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// TaskManager - Script principal
document.addEventListener('DOMContentLoaded', function() {
    // Elementos principales
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');
    const content = document.getElementById('content');
    
    // Función para controlar la visibilidad del sidebar
    function toggleSidebar() {
        if (!sidebar || !content) return;
        
        // Toggle sidebar
        sidebar.classList.toggle('collapsed');
        
        // Ajustar contenido
        content.classList.toggle('expanded');
        
        // Guardar preferencia en localStorage
        const isSidebarVisible = !sidebar.classList.contains('collapsed');
        localStorage.setItem('sidebarVisible', isSidebarVisible ? 'true' : 'false');
    }
    
    // Listener para el botón de toggle
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function(e) {
            e.preventDefault();
            toggleSidebar();
        });
    }
    
    // Cerrar sidebar al hacer click fuera en dispositivos móviles
    document.addEventListener('click', function(e) {
        if (window.innerWidth < 992 && 
            sidebar && 
            !sidebar.classList.contains('collapsed') && 
            !sidebar.contains(e.target) && 
            (!sidebarToggle || !sidebarToggle.contains(e.target))) {
            
            toggleSidebar();
        }
    });
    
    // Restaurar estado del sidebar desde localStorage
    function restoreSidebarState() {
        if (!sidebar || !content) return;
        
        const isSidebarVisible = localStorage.getItem('sidebarVisible');
        const isSmallScreen = window.innerWidth < 992;
        
        if (isSmallScreen || isSidebarVisible === 'false') {
            // En pantallas pequeñas o si el usuario lo ocultó previamente
            sidebar.classList.add('collapsed');
            content.classList.add('expanded');
        } else {
            // Por defecto en pantallas grandes, mostrar sidebar
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
            // En pantallas pequeñas, ocultar sidebar
            sidebar.classList.add('collapsed');
            if (content) content.classList.add('expanded');
        } else if (!isSmallScreen) {
            // En pantallas grandes, restaurar preferencia
            restoreSidebarState();
        }
    });
    
    // Inicializar tooltips de Bootstrap
    var tooltips = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltips.map(function(tooltip) {
        return new bootstrap.Tooltip(tooltip);
    });
    
    // Inicializar dropdowns de Bootstrap
    var dropdowns = [].slice.call(document.querySelectorAll('.dropdown-toggle'));
    dropdowns.map(function(dropdown) {
        return new bootstrap.Dropdown(dropdown);
    });
});
