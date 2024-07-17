import Navbar from '@/components/Navbar'
import { createRootRoute, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'

export const Route = createRootRoute({
	component: () => (
		<>
			<Navbar />
			<div className="container mx-auto my-4">
				<Outlet />
			</div>
			{process.env.NODE_ENV === 'development' && <TanStackRouterDevtools />}
		</>
	),
})
