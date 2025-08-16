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
        if (toggleSidebarBtn) {
            toggleSidebarBtn.querySelector('i').classList.remove('fa-bars');
            toggleSidebarBtn.querySelector('i').classList.add('fa-angles-right');
        }
    }
    
    if (toggleSidebarBtn) {
        toggleSidebarBtn.addEventListener('click', function() {
            body.classList.toggle('sidebar-collapsed');
            const isCollapsed = body.classList.contains('sidebar-collapsed');
            localStorage.setItem('sidebarCollapsed', isCollapsed);
            
            // Change icon based on sidebar state
            if (isCollapsed) {
                toggleSidebarBtn.querySelector('i').classList.remove('fa-bars');
                toggleSidebarBtn.querySelector('i').classList.add('fa-angles-right');
            } else {
                toggleSidebarBtn.querySelector('i').classList.remove('fa-angles-right');
                toggleSidebarBtn.querySelector('i').classList.add('fa-bars');
            }
        });
    }
    
    if (toggleBtn) {
        toggleBtn.addEventListener('click', function() {
            body.classList.add('sidebar-collapsed');
            localStorage.setItem('sidebarCollapsed', 'true');
        });
    }
    
    // Task filtering
    const filterButtons = document.querySelectorAll('.filter-button');
    if (filterButtons.length > 0) {
        filterButtons.forEach(button => {
            button.addEventListener('click', function() {
                // Remove active class from all buttons
                filterButtons.forEach(btn => btn.classList.remove('active'));
                
                // Add active class to clicked button
                this.classList.add('active');
                
                const filter = this.dataset.filter;
                filterTasks(filter);
            });
        });
    }
    
    // Task completion toggling
    const taskCheckboxes = document.querySelectorAll('.task-checkbox');
    if (taskCheckboxes.length > 0) {
        taskCheckboxes.forEach(checkbox => {
            checkbox.addEventListener('change', function() {
                const taskItem = this.closest('.task-item');
                const taskTitle = taskItem.querySelector('.task-title');
                const taskDescription = taskItem.querySelector('.task-description');
                
                if (this.checked) {
                    taskTitle.classList.add('task-completed');
                    taskDescription.classList.add('task-completed');
                } else {
                    taskTitle.classList.remove('task-completed');
                    taskDescription.classList.remove('task-completed');
                }
                
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
});

// Function to filter tasks based on status
function filterTasks(filter) {
    const tasks = document.querySelectorAll('.task-item');
    
    tasks.forEach(task => {
        const isCompleted = task.querySelector('.task-checkbox').checked;
        
        if (filter === 'all') {
            task.style.display = '';
        } else if (filter === 'completed' && isCompleted) {
            task.style.display = '';
        } else if (filter === 'pending' && !isCompleted) {
            task.style.display = '';
        } else {
            task.style.display = 'none';
        }
    });
}

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
            
            if (totalTasksElement && completedTasksElement && pendingTasksElement) {
                totalTasksElement.textContent = data.totalTasks;
                completedTasksElement.textContent = data.completedTasks;
                pendingTasksElement.textContent = data.pendingTasks;
                
                // Add animation to show that the stats have been updated
                const statsContainer = document.querySelector('.task-stats');
                statsContainer.classList.add('task-stats-refresh');
                
                // Remove animation class after animation completes
                setTimeout(() => {
                    statsContainer.classList.remove('task-stats-refresh');
                }, 1000);
            }
        }
    });
}
