import { Schema } from '@/api/client'
import { useDeleteMutation, usePutMutation } from '@/api/hooks'
import { Link } from '@tanstack/react-router'
import clsx from 'clsx'
import { Edit2Icon, TrashIcon } from 'lucide-react'
import { useState } from 'react'
import Modal from './Modal'
import ContactDetails from './ContactDetails'
import ContactEditor from './ContactEditor'
import { queryClient } from '@/main'
import useUser from '@/hooks/useUser'

export default function ContactRow({ contact }: { contact: Schema<'ContactDTO'> }) {
	const { mutate: mutateDelete, isPending: isPendingDelete } = useDeleteMutation('/Contact/{id}')
	const {
		mutate: mutateEdit,
		isPending: isPendingEdit,
		error: errorEdit,
	} = usePutMutation('/Contact/{id}', {})
	const [showDetails, setShowDetails] = useState(false)
	const [showEditor, setShowEditor] = useState(false)
	const { user } = useUser()

	const canModify = user && (!contact.registered || contact.id === user.id)

	return (
		<tr key={`contact${contact.id}`} className={clsx('hover', isPendingDelete && 'opacity-30')}>
			<td>{contact.firstName}</td>
			<td>{contact.lastName}</td>
			<td>{contact.phone}</td>
			<td className="flex gap-4">
				<Link to="/contact/$contactId" params={{ contactId: contact.id.toString() }}>
					<button
						className="btn btn-sm"
						disabled={isPendingDelete}
						onClick={e => {
							e.preventDefault(), setShowDetails(true)
						}}
					>
						Szczegóły
					</button>
				</Link>
				{canModify && (
					<div className="tooltip" data-tip="Edytuj kontakt">
						<Link to="/contact/$contactId/edit" params={{ contactId: contact.id.toString() }}>
							<button
								className="btn btn-sm btn-square btn-info"
								disabled={isPendingDelete}
								onClick={e => {
									e.preventDefault(), setShowEditor(true)
								}}
							>
								<Edit2Icon className="w-4 h-4" />
							</button>
						</Link>
					</div>
				)}
				{user && !contact.registered && (
					<div className="tooltip" data-tip="Usuń kontakt">
						<button
							className="btn btn-sm btn-square btn-error"
							disabled={isPendingDelete}
							onClick={() =>
								mutateDelete(
									{ params: { path: { id: contact.id } } },
									{
										onSuccess: () => void queryClient.invalidateQueries({ queryKey: ['/Contact'] }),
									}
								)
							}
						>
							{isPendingDelete ? (
								<span className="loading loading-spinner"></span>
							) : (
								<TrashIcon className="w-4 h-4" />
							)}
						</button>
					</div>
				)}
			</td>

			<Modal isOpen={showDetails} onClose={() => setShowDetails(false)} title="Szczegóły kontaktu">
				<ContactDetails contact={contact} />
			</Modal>

			{canModify && (
				<Modal isOpen={showEditor} onClose={() => setShowEditor(false)} title="Edytuj kontakt">
					<ContactEditor
						initialContact={contact}
						error={errorEdit}
						onCancel={() => setShowEditor(false)}
						onSave={contact =>
							mutateEdit(
								{ params: { path: { id: contact.id } }, body: contact },
								{
									onSuccess: () => {
										void queryClient
											.invalidateQueries({ queryKey: ['/Contact'] })
											.then(() => setShowEditor(false))
									},
								}
							)
						}
						disabled={isPendingEdit}
					/>
				</Modal>
			)}
		</tr>
	)
}
