<template>
	<div>
		<FlightsTable />
	</div>
</template>

<script>
import FlightsTable from '@/components/public/FlightsTable.vue'

export default {
	name: 'Home',
	
	components: {
		FlightsTable
	}
}
</script>
