import { createRouteMask, createRouter } from '@tanstack/react-router'
import { routeTree } from './routeTree.gen'

const contactModalMask = createRouteMask({
	routeTree,
	from: '/contact/$contactId/modal',
	to: '/contact/$contactId',
	params: true,
})

const contactEditModalMask = createRouteMask({
	routeTree,
	from: '/contact/$contactId/edit/modal',
	to: '/contact/$contactId/edit',
	params: true,
})

export const router = createRouter({
	routeTree,
	routeMasks: [contactModalMask, contactEditModalMask],
})

declare module '@tanstack/react-router' {
	interface Register {
		router: typeof router
	}
}
