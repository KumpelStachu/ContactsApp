import { usePostMutation } from '@/api/hooks'
import useUser from '@/hooks/useUser'
import { createLazyFileRoute, useNavigate } from '@tanstack/react-router'
import { KeyRoundIcon, MailIcon } from 'lucide-react'

export const Route = createLazyFileRoute('/login')({
	component: function Login() {
		const { mutate, error, isPending } = usePostMutation('/Auth/login')
		const navigate = useNavigate()

		function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
			e.preventDefault()
			const formData = new FormData(e.currentTarget)
			mutate(
				{
					body: {
						email: formData.get('email') as string,
						password: formData.get('password') as string,
					},
				},
				{
					onSuccess(data) {
						if (!data) return
						localStorage.setItem('token', data.token)
						useUser.setData(data.user)
						void navigate({ to: '/' })
					},
				}
			)
		}

		return (
			<div className="card bg-base-100 max-w-lg mx-auto mt-16">
				<form onSubmit={handleSubmit} className="card-body">
					<div className="card-title">Logowanie</div>
					{error && <div className="alert alert-error">{error.message}</div>}
					<label className="input input-bordered flex items-center gap-2">
						<MailIcon className="w-6 h-6" />
						<input type="email" name="email" className="grow" placeholder="Email" required />
					</label>
					<label className="input input-bordered flex items-center gap-2">
						<KeyRoundIcon className="w-6 h-6" />
						<input type="password" name="password" className="grow" placeholder="Hasło" required />
					</label>
					<button type="submit" className="btn btn-primary" disabled={isPending}>
						Zaloguj się
					</button>
				</form>
			</div>
		)
	},
})
