// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Task management functionality
document.addEventListener('DOMContentLoaded', function() {
    // Sidebar toggle
    const toggleSidebarBtn = document.getElementById('toggleSidebar');
    const body = document.body;
    const sidebar = document.querySelector('.sidebar');
    const toggleBtn = document.querySelector('.toggle-sidebar-btn');
    
    // Check if sidebar state is saved in localStorage
    const sidebarCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
    if (sidebarCollapsed) {
        body.classList.add('sidebar-collapsed');
    }
    
    if (toggleSidebarBtn) {
        toggleSidebarBtn.addEventListener('click', function() {
            body.classList.toggle('sidebar-collapsed');
            const isCollapsed = body.classList.contains('sidebar-collapsed');
            localStorage.setItem('sidebarCollapsed', isCollapsed);
        });
    }
    
    if (toggleBtn) {
        toggleBtn.addEventListener('click', function() {
            body.classList.add('sidebar-collapsed');
            localStorage.setItem('sidebarCollapsed', 'true');
        });
    }
    
    // Highlight active sidebar item
    const currentPath = window.location.pathname.toLowerCase();
    const sidebarItems = document.querySelectorAll('.sidebar-item');
    
    sidebarItems.forEach(item => {
        const href = item.getAttribute('href');
        if (href && currentPath.includes(href.toLowerCase())) {
            item.classList.add('active');
        }
    });
    
    // Task completion toggling
    const taskCheckboxes = document.querySelectorAll('.task-checkbox');
    if (taskCheckboxes.length > 0) {
        taskCheckboxes.forEach(checkbox => {
            checkbox.addEventListener('change', function() {
                updateTaskStatus(this.dataset.taskId, this.checked);
            });
        });
    }
    
    // Password visibility toggle
    const togglePasswordBtn = document.getElementById('togglePassword');
    if (togglePasswordBtn) {
        togglePasswordBtn.addEventListener('click', function() {
            const passwordInput = document.getElementById('password');
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);
            
            // Toggle the eye icon
            this.querySelector('i').classList.toggle('fa-eye');
            this.querySelector('i').classList.toggle('fa-eye-slash');
        });
    }
    
    // Add table row hover effect
    const tableRows = document.querySelectorAll('.task-table tbody tr');
    tableRows.forEach(row => {
        row.addEventListener('mouseenter', function() {
            this.style.backgroundColor = 'rgba(0,0,0,0.02)';
        });
        row.addEventListener('mouseleave', function() {
            this.style.backgroundColor = '';
        });
    });
});

// Function to update task status with AJAX and refresh statistics
function updateTaskStatus(taskId, isCompleted) {
    console.log(`Task ${taskId} is now ${isCompleted ? 'completed' : 'pending'}`);
    
    // AJAX call to update task status
    $.post('/Tasks?handler=ToggleStatus', { id: taskId }, function(data) {
        console.log('Status updated:', data);
        
        // Refresh task statistics if they exist on the page
        refreshTaskStatistics();
    });
}

// Function to refresh task statistics
function refreshTaskStatistics() {
    $.ajax({
        url: '/Index?handler=TaskStats',
        type: 'GET',
        success: function(data) {
            // If we have task statistics on the page, update them
            const totalTasksElement = document.querySelector('.task-stats .total-tasks');
            const completedTasksElement = document.querySelector('.task-stats .completed-tasks');
            const pendingTasksElement = document.querySelector('.task-stats .pending-tasks');
            const urgentTasksElement = document.querySelector('.task-stats .urgent-tasks');
            
            if (totalTasksElement) {
                totalTasksElement.textContent = data.totalTasks;
            }
            
            if (completedTasksElement) {
                completedTasksElement.textContent = data.completedTasks;
            }
            
            if (pendingTasksElement) {
                pendingTasksElement.textContent = data.pendingTasks;
            }
            
            if (urgentTasksElement) {
                urgentTasksElement.textContent = data.urgentTasks;
            }
            
            // Add animation to show that the stats have been updated
            const statsContainer = document.querySelector('.task-stats');
            if (statsContainer) {
                statsContainer.classList.add('task-stats-refresh');
                
                // Remove animation class after animation completes
                setTimeout(() => {
                    statsContainer.classList.remove('task-stats-refresh');
                }, 1000);
            }
        }
    });
}
