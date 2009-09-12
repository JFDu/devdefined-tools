specification @vacations:
	requires @scheduling_work	
	requires @external_connections
	
specification @scheduling_work:
	users_per_machine 100
	min_memory 4096
	min_cpu_count 2

specification @salary:	
	users_per_machine 150

specification @taxes:
	users_per_machine 50

specification @pension:
	if total_users < 1000:
		same_machine_as @health_insurance
