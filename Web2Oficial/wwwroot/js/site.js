// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function() {
    // Inicializar tooltips de Bootstrap si existen
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    if (typeof bootstrap !== 'undefined') {
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }
    
    // Efectos hover para las filas de la tabla
    const tableRows = document.querySelectorAll('.grid-item');
    
    tableRows.forEach(item => {
        item.addEventListener('mouseover', function() {
            this.style.transition = 'background-color 0.2s';
            this.style.backgroundColor = '#f0f0f0';
        });
        
        item.addEventListener('mouseout', function() {
            this.style.backgroundColor = '';
        });
    });

    // Mejorar apariencia del selector dropdown
    const pageSizeSelector = document.getElementById('pageSizeSelector');
    if (pageSizeSelector) {
        // Agregar evento change para la redirección
        pageSizeSelector.addEventListener('change', function() {
            window.location.href = '?pagina=1&tamanoPagina=' + this.value;
        });
        
        // Agregar efectos visuales al selector
        pageSizeSelector.addEventListener('focus', function() {
            this.style.borderColor = '#8b5cf6';
            this.style.boxShadow = '0 0 0 0.25rem rgba(139, 92, 246, 0.25)';
        });
        
        pageSizeSelector.addEventListener('blur', function() {
            this.style.boxShadow = 'none';
        });
        
        // Personalizar opciones del selector
        const options = pageSizeSelector.querySelectorAll('option');
        options.forEach(option => {
            // Establecer estilos base para todas las opciones
            option.style.backgroundColor = '#f5f3ff';
            option.style.color = '#5b21b6';
            option.style.padding = '10px';
            option.style.fontWeight = '500';
            option.style.borderBottom = '1px solid #e9d5ff';
            
            if (option.selected) {
                option.style.backgroundColor = '#8b5cf6';
                option.style.color = 'white';
            }
            
            // Agregar eventos de mouse para simular hover
            option.addEventListener('mouseover', function() {
                if (!this.selected) {
                    this.style.backgroundColor = '#e9d5ff';
                }
            });
            
            option.addEventListener('mouseout', function() {
                if (!this.selected) {
                    this.style.backgroundColor = '#f5f3ff';
                }
            });
        });
        
        // Observar cambios en el selector para mantener los estilos
        const observer = new MutationObserver(function(mutations) {
            mutations.forEach(function(mutation) {
                if (mutation.type === 'attributes' || mutation.type === 'childList') {
                    const options = pageSizeSelector.querySelectorAll('option');
                    options.forEach(option => {
                        if (option.selected) {
                            option.style.backgroundColor = '#8b5cf6';
                            option.style.color = 'white';
                        } else {
                            option.style.backgroundColor = '#f5f3ff';
                            option.style.color = '#5b21b6';
                        }
                    });
                }
            });
        });
        
        observer.observe(pageSizeSelector, { 
            attributes: true, 
            childList: true,
            subtree: true
        });
    }
});

// Inicializar componentes cuando el DOM esté cargado
document.addEventListener('DOMContentLoaded', function() {
    // Configuración del sidebar
    const sidebarToggle = document.getElementById('sidebarToggle');
    const body = document.body;
    const mainSidebar = document.querySelector('.main-sidebar');
    const logo = document.querySelector('.logo');
    
    // Verificar si se debe colapsar el sidebar al inicio (recuperar del localStorage)
    const sidebarState = localStorage.getItem('sidebarState');
    if (sidebarState === 'collapsed') {
        body.classList.add('sidebar-collapsed');
    }
    
    // Función para alternar el estado del sidebar
    function toggleSidebar() {
        // Toggle de la clase en el body
        body.classList.toggle('sidebar-collapsed');
        
        // Ajustar el logo según el estado del sidebar
        if (body.classList.contains('sidebar-collapsed')) {
            logo.classList.add('collapsed');
        } else {
            logo.classList.remove('collapsed');
        }
        
        // Guardar el estado en localStorage
        if (body.classList.contains('sidebar-collapsed')) {
            localStorage.setItem('sidebarState', 'collapsed');
        } else {
            localStorage.setItem('sidebarState', 'expanded');
        }
    }
    
    // Añadir evento al botón de toggle
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            toggleSidebar();
        });
    }
    
    // Manejo de clic fuera del sidebar para cerrarlo en móviles
    if (window.innerWidth <= 768) {
        document.addEventListener('click', function(event) {
            // Si el sidebar está visible en móvil y se hace clic fuera de él, cerrarlo
            if (body.classList.contains('sidebar-active') && 
                !mainSidebar.contains(event.target) && 
                !sidebarToggle.contains(event.target)) {
                body.classList.remove('sidebar-active');
            }
        });
        
        // En móviles, al hacer clic en el toggle, usamos la clase sidebar-active
        if (sidebarToggle) {
            sidebarToggle.addEventListener('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                body.classList.toggle('sidebar-active');
            });
        }
    }
    
    // Marcar la página actual como activa en el sidebar
    const currentPath = window.location.pathname;
    const sidebarLinks = document.querySelectorAll('.sidebar-menu a');
    
    sidebarLinks.forEach(link => {
        const href = link.getAttribute('href').replace('~', '');
        
        // Comprobar si es la página actual
        if (currentPath === '/' && href === '/Index') {
            link.classList.add('active');
        } else if (currentPath !== '/' && href !== '/Index' && currentPath.includes(href)) {
            link.classList.add('active');
        }
    });
    
    // Inicializar tooltips de Bootstrap
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    if (typeof bootstrap !== 'undefined') {
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }
    
    // Inicializar popovers de Bootstrap
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    if (typeof bootstrap !== 'undefined') {
        popoverTriggerList.map(function (popoverTriggerEl) {
            return new bootstrap.Popover(popoverTriggerEl);
        });
    }
    
    // Efecto hover para elementos de la tabla
    const tableRows = document.querySelectorAll('.table tbody tr');
    tableRows.forEach(row => {
        row.addEventListener('mouseenter', function() {
            this.style.backgroundColor = 'rgba(233, 213, 255, 0.1)';
        });
        
        row.addEventListener('mouseleave', function() {
            this.style.backgroundColor = '';
        });
    });
    
    // Animación de entrada para el contenido principal
    const mainContent = document.querySelector('main');
    if (mainContent) {
        mainContent.style.opacity = '0';
        mainContent.style.transform = 'translateY(10px)';
        
        setTimeout(() => {
            mainContent.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
            mainContent.style.opacity = '1';
            mainContent.style.transform = 'translateY(0)';
        }, 100);
    }
    
    // Forzar estilos personalizados para los select
    const allSelects = document.querySelectorAll('select.form-select-sm');
    allSelects.forEach(select => {
        select.addEventListener('mousedown', function(e) {
            // Esta línea es solo un truco para forzar el redibujado de los estilos
            document.body.style.overflow = document.body.style.overflow;
        });
    });
});
