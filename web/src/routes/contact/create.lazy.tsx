import { usePostMutation } from '@/api/hooks'
import ContactEditor from '@/components/ContactEditor'
import { createLazyFileRoute, useNavigate } from '@tanstack/react-router'

export const Route = createLazyFileRoute('/contact/create')({
	component: function CreateContact() {
		const navigate = useNavigate()
		const { mutate, error, isPending } = usePostMutation('/Contact')

		return (
			<div className="card bg-base-100 max-w-lg mx-auto mt-16 shadow-md">
				<div className="card-body">
					<div className="card-title">Dodaj kontakt</div>
					<ContactEditor
						error={error}
						disabled={isPending}
						onSave={contact => {
							mutate({ body: contact }, { onSuccess: () => void navigate({ to: '/' }) })
						}}
					/>
				</div>
			</div>
		)
	},
})
