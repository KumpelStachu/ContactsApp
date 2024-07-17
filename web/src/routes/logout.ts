import useUser from '@/hooks/useUser'
import { createFileRoute, redirect } from '@tanstack/react-router'

export const Route = createFileRoute('/logout')({
	beforeLoad() {
		localStorage.removeItem('token')
		useUser.setData(null)
		redirect({ to: '/', throw: true })
	},
})
