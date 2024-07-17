import { useGetQuery, usePostMutation } from '@/api/hooks'
import ContactEditor from '@/components/ContactEditor'
import TextInput from '@/components/TextInput'
import useDebounced from '@/components/useDebounced'
import useController from '@/hooks/useController'
import useUser from '@/hooks/useUser'
import { createLazyFileRoute, useNavigate } from '@tanstack/react-router'
import clsx from 'clsx'
import { KeyRoundIcon, MailIcon } from 'lucide-react'
import { useState } from 'react'

export const Route = createLazyFileRoute('/register')({
	component: function Register() {
		const navigate = useNavigate()
		const email = useController()
		const password = useController()
		const [tab, setTab] = useState(0)
		const debouncedEmail = useDebounced(email.value, 500)
		const { mutate, error, isPending } = usePostMutation('/Auth/register')
		const { data, isLoading } = useGetQuery(
			'/Contact/check',
			{ params: { query: { email: debouncedEmail } } },
			{ enabled: !!debouncedEmail, retry: false }
		)

		function handleFirstStep(e: React.FormEvent<HTMLFormElement>) {
			e.preventDefault()
			setTab(1)
		}

		return (
			<div className="card bg-base-100 max-w-lg mx-auto mt-16 shadow-md">
				<div className="card-body">
					<div className="card-title">Rejestracja</div>
					<div role="tablist" className="tabs tabs-boxed">
						<a
							role="tab"
							className={clsx('tab', tab === 0 && 'tab-active')}
							onClick={() => setTab(0)}
						>
							Dane logowania
						</a>
						<a role="tab" className={clsx('tab', tab === 1 && 'tab-active')}>
							Szczegóły
						</a>
					</div>

					{tab === 0 && (
						<form onSubmit={handleFirstStep}>
							<TextInput
								label="Email"
								name="email"
								type="email"
								icon={MailIcon}
								required
								{...email.props}
							/>
							<TextInput
								label="Hasło"
								name="password"
								type="password"
								autoComplete="new-password"
								icon={KeyRoundIcon}
								required
								{...password.props}
							/>
							<div className="flex items-center justify-between mt-4">
								<div className="text-error">{data?.registered && 'Email jest już zajęty'}</div>
								<div className="space-x-4">
									{isLoading && <div className="loading loading-dots"></div>}
									<button type="submit" className="btn btn-primary" disabled={data?.registered}>
										Dalej
									</button>
								</div>
							</div>
						</form>
					)}

					{tab === 1 && (
						<ContactEditor
							hideEmail
							initialContact={data}
							error={error}
							disabled={isPending}
							onCancel={() => setTab(0)}
							onSave={contact => {
								mutate(
									{ body: { ...contact, password: password.value, email: email.value } },
									{
										onSuccess: data => {
											if (!data) return
											localStorage.setItem('token', data.token)
											useUser.setData(data.user)
											void navigate({ to: '/' })
										},
									}
								)
							}}
						/>
					)}
				</div>
			</div>
		)
	},
})
